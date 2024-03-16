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
        progressImage.color = emptyColor;
        SetValue(0);

        EyeManager.Instance.lookInSelectable.AddListener(() => _canProcess = true);
        EyeManager.Instance.lookOutSelectable.AddListener(() => _canProcess = false);
        EyeManager.Instance.selectableSelected.AddListener(() => StartCoroutine(ResetDelay()));
    }

    private void OnDisable()
    {
        EyeManager.Instance.lookInSelectable.RemoveListener(() => _canProcess = true);
        EyeManager.Instance.lookOutSelectable.RemoveListener(() => _canProcess = false);
        EyeManager.Instance.selectableSelected.RemoveListener(() => StartCoroutine(ResetDelay()));
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

    private IEnumerator ResetDelay()
    {
        _canProcess = false;
        progressSlider.value = progressSlider.maxValue;
        progressImage.color = fullColor;

        yield return new WaitForSeconds(resetDelay);

        _canProcess = true;
        progressImage.color = emptyColor;
        progressSlider.value = 0f;
    }
}
