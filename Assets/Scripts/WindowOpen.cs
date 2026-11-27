
using UnityEngine;
using System.Collections;

public class WindowOpen : MonoBehaviour
{
    [Header("ウィンドウ")]
    [SerializeField] private RectTransform window;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private WindowClose closeController;

    [Header("ボタン")]
    [SerializeField] private RectTransform openButton;

    [Header("アニメーション設定")]
    [SerializeField] private float animationDuration = 0.4f;
    [SerializeField] private float startScale = 0.05f;

    private Coroutine animationCoroutine;

    // ウィンドウ本来の位置を記憶
    private Vector3 savedWindowPosition;

    private void Start()
    {
        savedWindowPosition = window.position;
    }

    // ボタンのOnClickから呼び出す
    public void ToggleWindow()
    {
        if (animationCoroutine != null)
            return;

        if (window.gameObject.activeSelf)
        {
            animationCoroutine = StartCoroutine(CloseToButton());
        }
        else
        {
            animationCoroutine = StartCoroutine(OpenFromButton());
        }
    }

    private IEnumerator OpenFromButton()
    {
        window.gameObject.SetActive(true);

        // 記憶している元の位置をそのまま使用
        Vector3 startPosition = openButton.position;

        window.position = startPosition;
        window.localScale = Vector3.one * startScale;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / animationDuration);
            float eased = Mathf.SmoothStep(0f, 1f, t);

            window.position = Vector3.Lerp(
                startPosition,
                savedWindowPosition,
                eased
            );

            window.localScale = Vector3.Lerp(
                Vector3.one * startScale,
                Vector3.one,
                eased
            );

            canvasGroup.alpha = eased;

            yield return null;
        }

        window.position = savedWindowPosition;
        window.localScale = Vector3.one;
        canvasGroup.alpha = 1f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        animationCoroutine = null;
    }

    private IEnumerator CloseToButton()
    {
        // 現在のウィンドウ位置を保存
        savedWindowPosition = window.position;

        Vector3 startPosition = window.position;
        Vector3 targetPosition = openButton.position;

        Vector3 startScale = window.localScale;
        Vector3 targetScale = Vector3.one * this.startScale;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(timer / animationDuration);
            float eased = Mathf.SmoothStep(0f, 1f, t);

            // ボタンの位置に向かって移動
            window.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                eased
            );

            // 縮小
            window.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                eased
            );

            // フェードアウト
            canvasGroup.alpha = 1f - eased;

            yield return null;
        }

        window.position = targetPosition;
        window.localScale = targetScale;
        canvasGroup.alpha = 0f;

        window.gameObject.SetActive(false);

        animationCoroutine = null;
    }
}
