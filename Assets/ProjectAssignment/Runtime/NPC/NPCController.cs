using UnityEngine;
using static GameManager;

public class NPCController : MonoBehaviour
{
    [Header("Wander Range")]
    [SerializeField] private float wanderRange = 3f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Wait Time (random)")]
    [SerializeField] private float minWaitTime = 1f;
    [SerializeField] private float maxWaitTime = 3f;

    [Header("Walk Time (random)")]
    [SerializeField] private float minWalkTime = 1f;
    [SerializeField] private float maxWalkTime = 3f;

    [Header("Player Detection")]
    [SerializeField] private InterActionZone zone;

    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 startPosition;
    private float moveDirection;
    private float stateTimer;

    [SerializeField] private bool isWalking;
    [SerializeField] private bool isMoving;

    private bool subscribedToGameManager;

    private void OnEnable()
    {
        if (zone != null)
            zone.OnInteraction += PlayInteractionOnStep;


        TrySubscribeToGameManager();
    }

    private void OnDisable()
    {
        if (zone != null)
            zone.OnInteraction -= PlayInteractionOnStep;

        if (subscribedToGameManager && GameManager.Instance != null)
        {
            GameManager.Instance.OnCurrentSpeaker -= CheckTalk;
            GameManager.Instance.OnStepChanged -= CheckStep;
            subscribedToGameManager = false;
        }
    }

    private void Awake()
    {
        startPosition = transform.position;

        StartWaiting();
    }

    private void Start()
    {
        TrySubscribeToGameManager();
    }

    private void TrySubscribeToGameManager()
    {
        if (subscribedToGameManager || GameManager.Instance == null)
            return;

        GameManager.Instance.OnCurrentSpeaker += CheckTalk;
        GameManager.Instance.OnStepChanged += CheckStep;
        subscribedToGameManager = true;
    }

    private void FixedUpdate()
    {
        if (IsPlayerInRange())
        {
            Stop();
            FacePlayer();
            CheckAnimation();
            return;
        }

        stateTimer -= Time.fixedDeltaTime;

        if (isWalking)
            Walk();
        else
            Wait();

        CheckAnimation();
    }

    private bool IsPlayerInRange()
    {
        return zone != null && zone.PlayerInRange;
    }

    private void FacePlayer()
    {
        if (zone == null || zone.Player == null)
            return;

        float direction = zone.Player.position.x - transform.position.x;
        if (direction != 0f)
            spriteRenderer.flipX = direction > 0f;
    }

    private void Walk()
    {
        float nextX = transform.position.x + moveDirection * moveSpeed * Time.fixedDeltaTime;

        if (nextX > startPosition.x + wanderRange || nextX < startPosition.x - wanderRange)
            moveDirection = -moveDirection;

        Vector2 velocity = rigidbody.linearVelocity;
        velocity.x = moveDirection * moveSpeed;
        rigidbody.linearVelocity = velocity;

        spriteRenderer.flipX = moveDirection > 0;
        isMoving = true;

        if (stateTimer <= 0f)
            StartWaiting();
    }

    private void Wait()
    {
        Stop();

        if (stateTimer <= 0f)
            StartWalking();
    }

    private void Stop()
    {
        Vector2 velocity = rigidbody.linearVelocity;
        velocity.x = 0f;
        rigidbody.linearVelocity = velocity;
        isMoving = false;
    }

    private void StartWaiting()
    {
        isWalking = false;
        stateTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    private void StartWalking()
    {
        isWalking = true;
        stateTimer = Random.Range(minWalkTime, maxWalkTime);
        moveDirection = Random.value < 0.5f ? -1f : 1f;
    }

    private void CheckAnimation()
    {
        if (animator != null)
            animator.SetBool("isMoving", isMoving);
    }

    private void PlayInteractionOnStep()
    {
        GameManager.Instance.PlayCurrentStep();
    }

    private void CheckTalk(string speaker)
    {
        if (speaker == "NPC")
        {
            animator.SetBool("isTalking", true);
        }
        else
        {
            animator.SetBool("isTalking", false);
        }
    }

    private void CheckStep(GameStep step)
    {
        if (step == GameStep.FirstTalk || step == GameStep.Delivery)
        {
            zone.gameObject.SetActive(true);
        }
        else
        {
            zone.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = Application.isPlaying ? (Vector3)startPosition : transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(center + Vector3.left * wanderRange, center + Vector3.right * wanderRange);
    }
}
