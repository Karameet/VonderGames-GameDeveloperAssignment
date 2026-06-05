using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class InterActionZone : MonoBehaviour
{
    [SerializeField] GameObject showButtonInterAction;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip interactClip;

    public event Action OnInteraction;

    private InputAction interactAction;
    [SerializeField] private bool playerInRange;

    public bool PlayerInRange => playerInRange;
    public Transform Player { get; private set; }

    private void Awake()
    {
        interactAction = InputSystem.actions.FindAction("Player/Interact");

        if (showButtonInterAction != null)
            showButtonInterAction.SetActive(false);
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.performed += OnInteractPerformed;
            interactAction.Enable();
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
            interactAction.performed -= OnInteractPerformed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerController _) &&
            other.GetComponentInParent<PlayerController>() == null)
            return;

        playerInRange = true;
        Player = other.transform;

        if (showButtonInterAction != null)
            showButtonInterAction.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerController _) &&
            other.GetComponentInParent<PlayerController>() == null)
            return;

        playerInRange = false;
        Player = null;

        if (showButtonInterAction != null)
            showButtonInterAction.SetActive(false);
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (!playerInRange)
            return;

        Interact();
    }

    private void Interact()
    {
        PlayInteractSound();
        OnInteraction?.Invoke();
    }

    private void PlayInteractSound()
    {
        if (sfxSource != null && interactClip != null)
            sfxSource.PlayOneShot(interactClip);
    }
}
