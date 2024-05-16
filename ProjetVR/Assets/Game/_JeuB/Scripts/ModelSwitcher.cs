using SignalSystem;
using UnityEngine;

public class ModelSwitcher : SignalListener
{
    [SerializeField] GameObject[] _skins;

    private GameObject _currentSkin;

    private void Start()
    {
        SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);

        foreach (var skin in _skins)
        {
            skin.SetActive(false);
        }

        _currentSkin = _skins[0];
        _currentSkin.SetActive(true);
    }

    protected override void OnSignalReceived(string value)
    {
        for (int i = 0; i < _skins.Length; i++)
        {
            Debug.Log("Signal: " + value);
            Debug.Log("Key: " + signal[i].name);
            if (value == signal[i].name)
            {
                _currentSkin.SetActive(false);
                _currentSkin = _skins[i];
                _currentSkin.SetActive(true);
                break;
            }
        }
    }
}
