using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private DialoguePanel dialoguePanel;

    [SerializeField] private DialogueData data;

    [Tooltip("ปุ่มสำหรับ skip / ไปบรรทัดถัดไป")]
    [SerializeField] private Button nextButton;

    [Header("Test")]
    [Tooltip("เล่น dialogue ทดสอบทันทีตอนเริ่มเกม")]
    [SerializeField] private bool playTestOnStart = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (playTestOnStart)
            PlayTestDialogue();
    }

    private void OnEnable()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnDisable()
    {
        if (nextButton != null)
            nextButton.onClick.RemoveListener(OnNextButtonClicked);
    }

    // กดปุ่มเพื่อ skip ถ้ายังพิมพ์ไม่จบ หรือไปบรรทัดถัดไป
    private void OnNextButtonClicked()
    {
        if (dialoguePanel != null && dialoguePanel.IsPlaying)
            dialoguePanel.Advance();
    }

    /// <summary>
    /// เล่น dialogue ทดสอบ — ใช้ data ที่ลากใส่ใน Inspector ถ้ามี
    /// ไม่งั้นสร้างชุดทดสอบขึ้นมาใน code
    /// </summary>
    public void PlayTestDialogue()
    {
        if (dialoguePanel == null)
        {
            Debug.LogWarning($"{nameof(GameManager)}: ยังไม่ได้ตั้งค่า dialoguePanel");
            return;
        }

        DialogueData dialogueToPlay = data;

        dialoguePanel.OnDialogueFinished -= HandleDialogueFinished;
        dialoguePanel.OnDialogueFinished += HandleDialogueFinished;
        dialoguePanel.Show(dialogueToPlay);
    }

    private void HandleDialogueFinished()
    {
        Debug.Log($"{nameof(GameManager)}: dialogue ทดสอบเล่นจบแล้ว");
    }
}
