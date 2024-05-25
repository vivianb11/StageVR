using SignalSystem;
using UnityEngine;

public class ModelSwitcher : SignalListener
{
    [SerializeField] GameObject[] _skins;

    private GameObject _currentSkin;

    private static int _skinIndex = 0;

    private void Start()
    {
        SignalManager.Instance.signalCalled.AddListener(OnSignalReceived);

        foreach (var skin in _skins)
        {
            skin.SetActive(false);
        }

        _currentSkin = _skins[_skinIndex];
        _currentSkin.SetActive(true);
    }

    protected override void OnSignalReceived(string value)
    {
        for (int i = 0; i < _skins.Length; i++)
        {
            if (value == signal[i].name)
            {
                _skinIndex = i;
                _currentSkin.SetActive(false);
                _currentSkin = _skins[i];
                _currentSkin.SetActive(true);
                break;
            }
        }
    }
}
