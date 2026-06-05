using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class InterActionZone : MonoBehaviour
{
    [SerializeField] GameObject showButtonInterAction;

    private event Action onInteract;

    private InputAction interactAction;
    [SerializeField] private bool playerInRange;

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

        if (showButtonInterAction != null)
            showButtonInterAction.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out PlayerController _) &&
            other.GetComponentInParent<PlayerController>() == null)
            return;

        playerInRange = false;

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
        onInteract?.Invoke();
        gameObject.SetActive(false);
    }
}
