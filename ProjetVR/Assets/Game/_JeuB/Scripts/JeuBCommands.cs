using SignalSystem;
using System.Collections.Generic;
using UnityEngine;

namespace JeuB
{
    [CreateAssetMenu(fileName = "JeuBCommands", menuName = "ScriptableObject/Commands/JeuB")]
    public class JeuBCommands : ScriptableObject
    {
        public static SO_Signal lastModeSignal;
        public static SO_Signal lastSkinSignal;

        public static Dictionary<string, GameObject> listeners { get; private set; } = new();
        public static Dictionary<string, bool> listenersActiveStats { get; private set; } = new();

        public static void AddKey(string key, GameObject gameObject)
        {
            listeners[key] = gameObject;
            listenersActiveStats[key] = gameObject.activeInHierarchy;
        }

        public void ToggleActiveState(string key)
        {
            if (listeners.ContainsKey(key))
            {
                listenersActiveStats[key] = !listenersActiveStats[key];
                listeners[key].SetActive(listenersActiveStats[key]);
            }
        }
    }
}
