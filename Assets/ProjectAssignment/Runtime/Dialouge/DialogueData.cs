using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]
public class DialogueData : ScriptableObject
{
    [SerializeField] private DialogueLine[] lines;

    public DialogueLine[] Lines => lines;

    // สร้าง DialogueData ตอน runtime (ใช้สำหรับ test โดยไม่ต้องสร้าง asset)
    //public static DialogueData Create(params DialogueLine[] lines)
    //{
    //    DialogueData data = CreateInstance<DialogueData>();
    //    data.lines = lines;
    //    return data;
    //}
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
