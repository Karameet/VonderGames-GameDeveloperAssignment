using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private DialoguePanel dialoguePanel;
    [SerializeField] private PlayableDirector director; 

    [Header("Steps")]
    [SerializeField] private Step[] steps;
    [SerializeField] private DialogueStep[] dialogueSteps;
    [SerializeField] private TimeLineStep[] timeLineSteps; 
    [SerializeField] private GameStep currentStep;

    private string currentSpeaker;

    public GameStep CurrentStep => currentStep;
    public string CurrentSpeaker => currentSpeaker;
    public static int StepCount => Enum.GetValues(typeof(GameStep)).Length;

    public event Action<GameStep> OnStepChanged;
    public event Action<string> OnCurrentSpeaker;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        steps = new Step[StepCount];
        for (int i = 0; i < steps.Length; i++)
            steps[i] = new Step((GameStep)i);

        ConfigureSteps();
    }

    private void ConfigureSteps()
    {
        var firstTalkStep = GetStep(GameStep.FirstTalk);
        firstTalkStep.onEnter = () => Debug.Log($"{nameof(GameManager)}: FirstTalk");
        firstTalkStep.onPlay = () =>
        {
            dialoguePanel.Show(dialogueSteps.FirstOrDefault(data => data.step == firstTalkStep.step).dialogueData);
            dialoguePanel.OnDialogueFinished += NextStep;
        };


        var itemFocus = GetStep(GameStep.ItemFocus);
        itemFocus.onEnter = () => { 
            Debug.Log($"{nameof(GameManager)}: ItemFocus");         
            director.Play(timeLineSteps.FirstOrDefault(data => data.step == itemFocus.step).clip);           
        } ;

        var delivery = GetStep(GameStep.Delivery);
        delivery.onEnter = () => Debug.Log($"{nameof(GameManager)}: Delivery");
        delivery.onPlay = () =>
        {
            dialoguePanel.Show(dialogueSteps.FirstOrDefault(data => data.step == delivery.step).dialogueData);
            dialoguePanel.OnDialogueFinished += NextStep;
        };


        var endEvent = GetStep(GameStep.EndEvent);
        endEvent.onEnter = () => { 
            Debug.Log($"{nameof(GameManager)}: EndEvent");
            director.Play(timeLineSteps.FirstOrDefault(data => data.step == endEvent.step).clip);
        };

    }

    private void Start()
    {
        EnterStep((GameStep)0);
    }
    private void OnEnable()
    {
        dialoguePanel.OnChangeSpeaker += OnSpeakerChange;
    }

    private void OnDisable()
    {
        dialoguePanel.OnChangeSpeaker -= OnSpeakerChange;
    }

    public void NextStep()
    {
        int next = (int)currentStep + 1;

        if (next >= StepCount)
        {
            Debug.Log($"{nameof(GameManager)}: Complete");
            return;
        }

        var stepData = GetStep(currentStep);
        if (stepData != null)
        {
            stepData.onExit?.Invoke();
        }

        EnterStep((GameStep)next);
    }

    public void EnterStep(GameStep step)
    {
        currentStep = step;
        OnStepChanged?.Invoke(step);

        var stepData = GetStep(currentStep);
        if (stepData != null)
        {
            stepData.onEnter?.Invoke();
        }
    }

    private Step GetStep(GameStep step)
    {
        foreach (Step s in steps)
        {
            if (s.step == step)
            {
                return s;
            }
        }
        return null;
    }

    public void PlayCurrentStep()
    {
        PlayStep(currentStep);
    }

    public void PlayStep(GameStep step)
    {
        var stepData = GetStep(step);
        if (stepData != null)
        {
            stepData.onPlay?.Invoke();
        }
    }


    private void OnSpeakerChange(string speaker)
    {
        currentSpeaker = speaker;
        OnCurrentSpeaker?.Invoke(speaker);
    }
}

public enum GameStep
{
    FirstTalk,
    ItemFocus,
    Delivery,
    EndEvent
}

[Serializable]
public class Step
{
    public GameStep step;
    public Action onEnter;
    public Action onPlay;
    public Action onExit;

    public Step(GameStep step)
    {
        this.step = step;
    }
}

[Serializable]
public class DialogueStep
{
    public GameStep step;
    public DialogueData dialogueData;
}

[Serializable]
public class TimeLineStep
{
    public GameStep step;
    public PlayableAsset clip;
}
