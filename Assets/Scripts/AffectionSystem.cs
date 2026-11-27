
using UnityEngine;
using TMPro;
using System.Collections;

public class AffectionSystem : MonoBehaviour
{
    [Header("好感度")]
    [SerializeField] private float affection = 0f;

    [Header("増減設定")]
    [SerializeField] private float changeSpeed = 10f;
    [SerializeField] private KeyCode increaseKey = KeyCode.Space;
    [SerializeField] private KeyCode decreaseKey = KeyCode.Z;

    [Header("表示")]
    [SerializeField] private TMP_Text affectionText;

    [Header("数字のアニメーション")]
    [SerializeField] private float shrinkScale = 0.85f;
    [SerializeField] private float animationDuration = 0.12f;

    private Vector3 originalScale;
    private Coroutine scaleCoroutine;
    private int previousValue;

    private void Start()
    {
        originalScale = affectionText.transform.localScale;
        previousValue = Mathf.FloorToInt(affection);
        UpdateText();
    }

    private void Update()
    {
        if (Input.GetKey(increaseKey))
            affection += changeSpeed * Time.deltaTime;

        if (Input.GetKey(decreaseKey))
            affection -= changeSpeed * Time.deltaTime;

        affection = Mathf.Clamp(affection, -100f, 100f);

        int currentValue = Mathf.FloorToInt(affection);

        // 整数部分が変化したときだけ反応
        if (currentValue != previousValue)
        {
            previousValue = currentValue;

            UpdateText();

            if (scaleCoroutine != null)
                StopCoroutine(scaleCoroutine);

            scaleCoroutine = StartCoroutine(ScaleAnimation());
        }
    }

    private void UpdateText()
    {
        affectionText.text = Mathf.FloorToInt(affection).ToString();
    }

    private IEnumerator ScaleAnimation()
    {
        float halfDuration = animationDuration / 2f;
        Vector3 smallScale = originalScale * shrinkScale;

        float timer = 0f;

        // 縮小
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / halfDuration);

            affectionText.transform.localScale =
                Vector3.Lerp(originalScale, smallScale, t);

            yield return null;
        }

        timer = 0f;

        // 元のサイズに戻る
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / halfDuration);

            affectionText.transform.localScale =
                Vector3.Lerp(smallScale, originalScale, t);

            yield return null;
        }

        affectionText.transform.localScale = originalScale;
        scaleCoroutine = null;
    }
}
