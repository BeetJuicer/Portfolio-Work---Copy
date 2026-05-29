using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class SliderUIListener : MonoBehaviour
{
    [SerializeField] Slider slider;
    TextMeshProUGUI display;

    [SerializeField] bool roundToInt;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        display = GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        if (roundToInt) 
            value = Mathf.RoundToInt(value);

        display.text = value.ToString();
    }
}
