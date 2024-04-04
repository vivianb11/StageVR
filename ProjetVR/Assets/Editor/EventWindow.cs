using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EventWindow : EditorWindow
{
    private Dictionary<Component, SerializedObject> events = new Dictionary<Component, SerializedObject>();
    private Dictionary<Component, bool> foldout = new Dictionary<Component, bool>();
    private Dictionary<GameObject, bool> GameObjectFoldout = new Dictionary<GameObject, bool>();

    private Vector2 scrollPos;

    [MenuItem("Window/Events")]
    public static void ShowWindow()
    {
        GetWindow<EventWindow>("Events");
    }

    private void OnGUI()
    {
        GUILayout.Label("Events", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        foreach (var pair in events)
        {
            var component = pair.Key;
            var gObj = component.gameObject;
            var serializedObject = pair.Value;

            if (!GameObjectFoldout.ContainsKey(gObj))
            {
                GameObjectFoldout.Add(component.gameObject, true);
            }

            GameObjectFoldout[gObj] = EditorGUILayout.Foldout(GameObjectFoldout[gObj], gObj.name);

            if (GameObjectFoldout[gObj])
            {
                if (!foldout.ContainsKey(component))
                {
                    foldout.Add(component, true);
                }

                foldout[component] = EditorGUILayout.InspectorTitlebar(foldout[component], component);

                if (foldout[component])
                {
                    serializedObject.Update();

                    foreach (FieldInfo field in component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (field.FieldType == typeof(UnityEvent))
                        {
                            var property = serializedObject.FindProperty(field.Name);
                            EditorGUILayout.PropertyField(property, true);
                        }
                    }

                    serializedObject.ApplyModifiedProperties();
                }
            }
        }

        EditorGUILayout.EndScrollView();

        Repaint();
    }

    private void OnSelectionChange()
    {
        events.Clear();
        foldout.Clear();

        foreach (var obj in Selection.gameObjects)
        {
            foreach (var component in obj.GetComponents<Component>())
            {
                foreach (var field in component.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (field.FieldType == typeof(UnityEvent))
                    {
                        if (!events.ContainsKey(component))
                        {
                            events.Add(component, new SerializedObject(component));
                            break;
                        }
                    }
                }
            }
        }
    }
}
