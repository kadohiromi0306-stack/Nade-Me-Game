
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HeartSpawner : MonoBehaviour, IPointerClickHandler
{
    [Header("ハート")]
    [SerializeField] private GameObject heartPrefab;

    [Header("生成先")]
    [SerializeField] private RectTransform canvasRect;

    [Header("出現設定")]
    [SerializeField] private float minSize = 0.6f;
    [SerializeField] private float maxSize = 1.3f;
    [SerializeField] private float duration = 0.55f;

    [Header("飛び散り設定")]
    [SerializeField] private float minSpeed = 180f;
    [SerializeField] private float maxSpeed = 350f;

    [Header("複数出現")]
    [SerializeField] private float multipleChance = 0.2f;

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 localPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPosition
        );

        int count = Random.value < multipleChance ? 3 : 1;

        for (int i = 0; i < count; i++)
        {
            SpawnHeart(localPosition);
        }
    }

    private void SpawnHeart(Vector2 position)
    {
        GameObject heart = Instantiate(heartPrefab, canvasRect);

        RectTransform rect = heart.GetComponent<RectTransform>();

        rect.anchoredPosition = position;

        float size = Random.Range(minSize, maxSize);
        rect.localScale = Vector3.zero;

        float angle = Random.Range(35f, 145f);

        if (Random.value < 0.5f)
            angle = 180f - angle;

        Vector2 direction = new Vector2(
            Mathf.Cos(angle * Mathf.Deg2Rad),
            Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        float speed = Random.Range(minSpeed, maxSpeed);

        // ★ これが抜けていた！
        StartCoroutine(HeartAnimation(
            heart, rect, direction, speed, size
        ));
    }

    private IEnumerator HeartAnimation(
        GameObject heart,
        RectTransform rect,
        Vector2 direction,
        float speed,
        float size)
    {
        CanvasGroup group = heart.GetComponent<CanvasGroup>();

        if (group == null)
            group = heart.AddComponent<CanvasGroup>();

        Vector3 targetScale = Vector3.one * size;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            // ポンッと大きくなる
            float pop = Mathf.Sin(
                Mathf.Min(t * 2f, 1f) * Mathf.PI * 0.5f
            );

            rect.localScale = Vector3.Lerp(
                Vector3.zero,
                targetScale,
                pop
            );

            // ランダムな方向へ飛び散る
            rect.anchoredPosition +=
                direction * speed * Time.deltaTime;

            // 徐々に透明になる
            group.alpha = 1f - t;

            yield return null;
        }

        Destroy(heart);
    }
}
