using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SelectionProgress : MonoBehaviour
{
    [SerializeField] Slider progressSlider;
    [SerializeField] Image progressImage;

    [SerializeField] Color emptyColor = Color.white;
    [SerializeField] Color fullColor = Color.green;

    [SerializeField] float resetDelay = 1f;
    [SerializeField] float lerpSpeed = 0.5f;

    private bool _canProcess;

    private void Start()
    {
        Reset();

        EyeManager.Instance.lookInSelectable.AddListener(OnLookIn);
        EyeManager.Instance.lookOutSelectable.AddListener(Reset);
        EyeManager.Instance.selectableSelected.AddListener(OnSelected);
    }

    private void OnDisable()
    {
        EyeManager.Instance.lookInSelectable.RemoveListener(OnLookIn);
        EyeManager.Instance.lookOutSelectable.RemoveListener(Reset);
        EyeManager.Instance.selectableSelected.RemoveListener(OnSelected);
    }

    private void FixedUpdate()
    {
        if (!_canProcess)
            return;

        SetMaxValue(EyeManager.Instance.maxValue);
        SetValue(EyeManager.Instance.value);
    }

    public void SetMaxValue(float value)
    {
        progressSlider.maxValue = value;
    }

    public void SetValue(float value)
    {
        float clampedValue = Mathf.Clamp(value, progressSlider.minValue, progressSlider.maxValue);

        progressSlider.value = Mathf.Lerp(progressSlider.value, clampedValue, lerpSpeed);
    }

    public void AddValue(float value)
    {
        SetValue(progressSlider.value + value);
    }

    private void OnLookIn()
    {
        _canProcess = true;
    }

    private void Reset()
    {
        _canProcess = false;
        progressImage.color = emptyColor;
        progressSlider.value = 0f;
    }

    private void OnSelected()
    {
        StartCoroutine(ResetDelay());
    }

    private IEnumerator ResetDelay()
    {
        _canProcess = false;
        progressSlider.value = progressSlider.maxValue;
        progressImage.color = fullColor;

        yield return new WaitForSeconds(resetDelay);

        Reset();
        _canProcess = true;
    }
}
