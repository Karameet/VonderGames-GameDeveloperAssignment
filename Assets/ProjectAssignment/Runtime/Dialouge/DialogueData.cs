using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private DialogueLine[] lines;

    public DialogueLine[] Lines => lines;
}

[Serializable]
public struct DialogueLine
{
    public string speaker;

    [TextArea(2, 5)]
    public string message;
    public Sprite leftPortrait;
    public Sprite rightPortrait;
}
