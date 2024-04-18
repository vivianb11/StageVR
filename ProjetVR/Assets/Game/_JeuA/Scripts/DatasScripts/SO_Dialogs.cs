using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogs", menuName = "ScriptableObject/Dialogs")]
public class SO_Dialogs : ScriptableObject
{
    [TextArea]
    public string content;
}
