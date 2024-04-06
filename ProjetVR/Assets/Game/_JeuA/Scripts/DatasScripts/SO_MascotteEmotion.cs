using UnityEngine;

[CreateAssetMenu(fileName = "New Emotion", menuName = "ScriptableObject/MascotteEmotion")]
public class SO_MascotteEmotion : ScriptableObject
{
    public Sprite[] sprites;

    public Sound[] sounds;
}
