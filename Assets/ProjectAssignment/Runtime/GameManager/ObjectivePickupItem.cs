using UnityEngine;

public class ObjectivePickupItem : MonoBehaviour
{
    [SerializeField] private InterActionZone interActionZone;

    private bool subscribedToGameManager;

    private void OnEnable()
    {
        if (interActionZone != null)
            interActionZone.OnInteraction += PlayInteractionOnStep;


        TrySubscribeToGameManager();
    }

    private void OnDisable()
    {
        if (interActionZone != null)
            interActionZone.OnInteraction -= PlayInteractionOnStep;

        if (subscribedToGameManager && GameManager.Instance != null)
        {

            subscribedToGameManager = false;
        }
    }

    private void Start()
    {
        TrySubscribeToGameManager();
    }

    private void TrySubscribeToGameManager()
    {
        if (subscribedToGameManager || GameManager.Instance == null)
            return;


        subscribedToGameManager = true;
    }

    private void PlayInteractionOnStep()
    {
        this.gameObject.SetActive(false);

        GameManager.Instance.NextStep();
    }
}
