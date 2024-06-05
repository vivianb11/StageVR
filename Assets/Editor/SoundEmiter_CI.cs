using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundEmiter))]
public class SoundEmiter_CI : Editor
{
    SoundEmiter soundEmiter;
    public bool fadeResume = true;
    public bool fadePause = true;
    public bool fadeStop = true;

    public void OnEnable()
    {
        soundEmiter = (SoundEmiter)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (soundEmiter.playRandomSfx)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sfxs"), true);
        }

        serializedObject.ApplyModifiedProperties();

        GUILayout.Space(10);

        if (EditorApplication.isPlaying)
        {
            SoundManager soundManager = SoundManager.Instance;

            // adds a slider that changes depending on the completion of the sound

            if (soundEmiter.lastPlayedSound >= 0)
            {
                if (soundManager._audioSources?.ToArray()[soundEmiter.lastPlayedSound].Key.time > 0)
                {
                    EditorGUILayout.Slider("Sound Completion", soundManager._audioSources.ToArray()[soundEmiter.lastPlayedSound].Key.time / soundManager._audioSources.ToArray()[soundEmiter.lastPlayedSound].Key.clip.length, 0, 1);

                    Repaint();
                }
                else
                {
                    EditorGUILayout.Slider("Sound Completion", 0, 0, 1);
                }
            }
            else if (soundManager._musicSource.time > 0)
            {
                EditorGUILayout.Slider("Music Completion", soundManager._musicSource.time / soundManager._musicSource.clip.length, 0, 1);

                Repaint();
            }
            else
            {
                EditorGUILayout.Slider("Music Completion", 0, 0, 1);
            }
        }

        GUILayout.Space(10);

        // dissables the button if not in play mode

        GUI.enabled = EditorApplication.isPlaying;

        if (GUILayout.Button("Play Sound"))
        {
            soundEmiter.PlaySound();
        }

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical();

        // add a bool with fade or not
        if (GUILayout.Button("Resume"))
        {
            soundEmiter.ResumeSound(fadeResume);
        }

        if (fadeResume)
        {
            GUI.backgroundColor = Color.green;
        }
        else
        {
            GUI.backgroundColor = Color.red;
        }

        GUI.enabled = true;

        if (GUILayout.Button("With Fade"))
        {
            fadeResume = !fadeResume;
        }

        GUI.enabled = EditorApplication.isPlaying;

        GUI.backgroundColor = Color.white;

        GUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Pause"))
        {
            soundEmiter.PauseSound(fadePause);
        }

        if (fadePause)
        {
            GUI.backgroundColor = Color.green;
        }
        else
        {
            GUI.backgroundColor = Color.red;
        }

        GUI.enabled = true;

        if (GUILayout.Button("With Fade"))
        {
            fadePause = !fadePause;
        }

        GUI.enabled = EditorApplication.isPlaying;

        GUI.backgroundColor = Color.white;

        GUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        if (soundEmiter.lastPlayedSound >= 0)
        {
            if (GUILayout.Button("Stop"))
            {
                soundEmiter.StopSound(fadeStop);
            }

            if (fadeStop)
            {
                GUI.backgroundColor = Color.green;
            }
            else
            {
                GUI.backgroundColor = Color.red;
            }

            GUI.enabled = true;

            if (GUILayout.Button("With Fade"))
            {
                fadeStop = !fadeStop;
            }
        }
        else
        {
            if (GUILayout.Button("Skip"))
            {
                soundEmiter.StopSound(false);
            }
        }

        GUI.enabled = EditorApplication.isPlaying;

        GUI.backgroundColor = Color.white;

        GUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }
}
