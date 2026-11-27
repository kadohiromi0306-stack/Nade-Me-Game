
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSettings : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TMP_Text bgmText;

    [Header("SE")]
    [SerializeField] private Slider seSlider;
    [SerializeField] private TMP_Text seText;

    void Start()
    {
        // スライダーの初期設定
        bgmSlider.minValue = 0;
        bgmSlider.maxValue = 100;
        bgmSlider.wholeNumbers = true;

        seSlider.minValue = 0;
        seSlider.maxValue = 100;
        seSlider.wholeNumbers = true;

        // 初期値
        bgmSlider.value = 100;
        seSlider.value = 100;

        // 数値表示を更新
        bgmSlider.onValueChanged.AddListener(UpdateBGM);
        seSlider.onValueChanged.AddListener(UpdateSE);

        UpdateBGM(bgmSlider.value);
        UpdateSE(seSlider.value);
    }

    void UpdateBGM(float value)
    {
        bgmText.text = Mathf.RoundToInt(value) + "%";
    }

    void UpdateSE(float value)
    {
        seText.text = Mathf.RoundToInt(value) + "%";
    }
}
