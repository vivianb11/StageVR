using SignalSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    [CreateAssetMenu(fileName = "JeuBCommands", menuName = "ScriptableObject/Commands/JeuB")]
    public class JeuBCommands : ScriptableObject
    {
        public static SO_Signal lastModeSignal;
        public static SO_Signal lastSkinSignal;

        public static Dictionary<string, GameObject> listeners { get; private set; } = new();
        public static Dictionary<string, bool> listenersActiveStats { get; private set; } = new();

        public UnityEvent OnToggleChange;

        public static void AddKey(string key, GameObject gameObject)
        {
            listeners[key] = gameObject;

            if (gameObject != null)
                listenersActiveStats[key] = gameObject.activeInHierarchy;
            else listenersActiveStats[key] = false;
        }

        public void ToggleActiveState(string key)
        {
            if (listeners.ContainsKey(key))
            {
                listenersActiveStats[key] = !listenersActiveStats[key];

                if (listeners[key] != null)
                    listeners[key].SetActive(listenersActiveStats[key]);
            }
        }

        public void EnableObject(string key)
        {
            if (listeners.ContainsKey(key))
            {
                listenersActiveStats[key] = true;

                if (listeners[key] != null)
                    listeners[key].SetActive(true);
            }
        }

        public void DisableObject(string key)
        {
            if (listeners.ContainsKey(key))
            {
                listenersActiveStats[key] = false;

                if (listeners[key] != null)
                    listeners[key].SetActive(false);
            }
        }

        public static void SetObject(string key, bool value)
        {
            if (listeners.ContainsKey(key))
            {
                listenersActiveStats[key] = value;

                if (listeners[key] != null)
                    listeners[key].SetActive(value);
            }
        }
    }
}
