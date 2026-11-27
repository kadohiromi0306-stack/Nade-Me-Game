
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class WindowClose : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("ウィンドウ")]
    [SerializeField] private RectTransform window;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("縮小先")]
    [SerializeField] private RectTransform mainWindowCenter;

    [Header("×画像")]
    [SerializeField] private Image normalImage;
    [SerializeField] private Image hoverImage;

    [Header("アニメーション")]
    [SerializeField] private float closeDuration = 0.4f;

    [Header("クリック音")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;

    private bool isClosing = false;

    private Vector3 savedPosition;

    public Vector3 SavedPosition => savedPosition;

    private void OnEnable()
    {
        isClosing = false;

        if (window != null)
            window.localScale = Vector3.one;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        // 通常画像を表示
        if (normalImage != null)
            normalImage.enabled = true;

        // 赤い画像を非表示
        if (hoverImage != null)
            hoverImage.enabled = false;
    }

    // クリックで閉じる
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClosing) return;

        // クリック音を再生
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        StartCoroutine(CloseAnimation());
    }

    // マウスを乗せたとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isClosing) return;

        if (normalImage != null)
            normalImage.enabled = false;

        if (hoverImage != null)
            hoverImage.enabled = true;
    }

    // マウスを外したとき
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isClosing) return;

        if (normalImage != null)
            normalImage.enabled = true;

        if (hoverImage != null)
            hoverImage.enabled = false;
    }

    private IEnumerator CloseAnimation()
    {
        isClosing = true;

        Vector3 startPosition = window.position;

        // 閉じる前の位置を保存
        savedPosition = startPosition;

        Vector3 startScale = window.localScale;

        // 縮小先の中心座標を取得
        Vector3[] corners = new Vector3[4];
        mainWindowCenter.GetWorldCorners(corners);

        Vector3 targetPosition =
            (corners[0] + corners[2]) / 2f;

        float timer = 0f;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (timer < closeDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(
                timer / closeDuration
            );

            float smooth = Mathf.SmoothStep(0f, 1f, t);

            // 中心に移動
            window.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                smooth
            );

            // 縮小
            window.localScale = Vector3.Lerp(
                startScale,
                Vector3.zero,
                smooth
            );

            // フェードアウト
            canvasGroup.alpha = 1f - smooth;

            yield return null;
        }

        window.gameObject.SetActive(false);
    }
}
