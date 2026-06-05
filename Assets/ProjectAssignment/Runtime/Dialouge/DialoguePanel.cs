using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] private DialogueBox dialogueBox;

    [SerializeField] private GameObject root;

    [SerializeField] private Button overlayButton;

    [Header("Slide Animation")]
    [SerializeField] private RectTransform slideTarget;
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private float hiddenOffsetY = -600f;
    [SerializeField] private Ease showEase = Ease.OutCubic;
    [SerializeField] private Ease hideEase = Ease.InCubic;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip newLineClip;

    private DialogueLine[] lines;
    private int currentIndex;
    private bool isPlaying;

    private Vector2 shownPosition;
    private Vector2 hiddenPosition;
    private Tween slideTween;

    public event Action<string> OnChangeSpeaker;

    private void Awake()
    {
        shownPosition = slideTarget.anchoredPosition;
        hiddenPosition = shownPosition + new Vector2(0f, hiddenOffsetY);
    }

    public event Action OnDialogueFinished;

    public bool IsPlaying => isPlaying;

    private void OnEnable()
    {
        if (dialogueBox != null)
            dialogueBox.OnTypingComplete += HandleTypingComplete;

        if (overlayButton != null)
            overlayButton.onClick.AddListener(Advance);
    }

    private void OnDisable()
    {
        if (dialogueBox != null)
            dialogueBox.OnTypingComplete -= HandleTypingComplete;

        if (overlayButton != null)
            overlayButton.onClick.RemoveAllListeners();

        slideTween?.Kill();
    }

    public void Show(DialogueData data)
    {
        if (data == null || data.Lines == null || data.Lines.Length == 0)
        {
            Debug.LogWarning($"{nameof(DialoguePanel)}: Don't have data ");
            return;
        }

        lines = data.Lines;
        currentIndex = 0;
        isPlaying = true;

        root.SetActive(true);

        slideTween?.Kill();
        slideTarget.anchoredPosition = hiddenPosition;
        slideTween = slideTarget.DOAnchorPos(shownPosition, slideDuration)
            .SetEase(showEase)
            .OnComplete(() => ShowLine(currentIndex));
    }

    public void Advance()
    {
        if (!isPlaying)
            return;

        if (dialogueBox.IsTyping)
        {
            dialogueBox.Skip();
            return;
        }

        currentIndex++;

        if (currentIndex >= lines.Length)
            Hide();
        else
            ShowLine(currentIndex);
    }

    public void Hide()
    {
        if (!isPlaying)
            return;

        isPlaying = false;

        slideTween?.Kill();
        slideTween = slideTarget.DOAnchorPos(hiddenPosition, slideDuration)
            .SetEase(hideEase)
            .OnComplete(() =>
            {
                root.SetActive(false);
                OnDialogueFinished?.Invoke();
                OnChangeSpeaker?.Invoke(string.Empty);
            });
    }

    private void ShowLine(int index)
    {
        DialogueLine line = lines[index];
        dialogueBox.SetPortraits(line.leftPortrait, line.rightPortrait);
        dialogueBox.ShowDialogue(line.speaker, line.message);
        OnChangeSpeaker?.Invoke(line.speaker);
        PlayNewLineSound();
    }

    private void PlayNewLineSound()
    {
        if (sfxSource != null && newLineClip != null)
            sfxSource.PlayOneShot(newLineClip);
    }

    private void HandleTypingComplete()
    {

    }
}
