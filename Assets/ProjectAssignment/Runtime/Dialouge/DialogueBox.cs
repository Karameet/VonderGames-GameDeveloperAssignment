using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI characterText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;

    [SerializeField] private float typingSpeed = 0.03f;

    private Coroutine typingRoutine;

    public event Action OnTypingComplete;

    public bool IsTyping { get; private set; }

    public void ShowDialogue(string speaker, string message)
    {
        if (characterText != null)
            characterText.text = speaker;

        StartTyping(message);
    }

    private void StartTyping(string message)
    {
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        typingRoutine = StartCoroutine(TypeRoutine(message));
    }

    private IEnumerator TypeRoutine(string message)
    {
        IsTyping = true;

        dialogueText.text = message;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.ForceMeshUpdate();

        int totalCharacters = dialogueText.textInfo.characterCount;

        WaitForSeconds wait = new WaitForSeconds(typingSpeed);

        for (int visible = 1; visible <= totalCharacters; visible++)
        {
            dialogueText.maxVisibleCharacters = visible;

            if (typingSpeed > 0f)
                yield return wait;
        }

        CompleteTyping();
    }

    public void Skip()
    {
        if (!IsTyping)
            return;

        if (typingRoutine != null)
        {
            StopCoroutine(typingRoutine);
            typingRoutine = null;
        }

        dialogueText.maxVisibleCharacters = int.MaxValue;
        CompleteTyping();
    }

    private void CompleteTyping()
    {
        typingRoutine = null;
        IsTyping = false;
        OnTypingComplete?.Invoke();
    }

    public void SetPortraits(Sprite left, Sprite right)
    {
        SetPortrait(leftImage, left);
        SetPortrait(rightImage, right);
    }

    private static void SetPortrait(Image target, Sprite sprite)
    {
        if (target == null)
            return;

        target.sprite = sprite;
        target.gameObject.SetActive(sprite != null);
    }
}
