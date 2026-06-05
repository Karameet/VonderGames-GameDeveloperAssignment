using DG.Tweening;
using UnityEngine;

public class ScaleLoop : MonoBehaviour
{
    [Header("Scale Animation")]
    [SerializeField] private Transform target;
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.InOutSine;
    [SerializeField] private bool playOnEnable = true;

    private Vector3 baseScale;
    private Tween scaleTween;

    private void Awake()
    {
        if (target == null)
            target = transform;

        baseScale = target.localScale;
    }

    private void OnEnable()
    {
        if (playOnEnable)
            Play();
    }

    private void OnDisable()
    {
        Stop();
    }

    public void Play()
    {
        scaleTween?.Kill();

        target.localScale = baseScale;
        scaleTween = target.DOScale(baseScale * scaleMultiplier, duration)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Stop()
    {
        scaleTween?.Kill();
        scaleTween = null;
        target.localScale = baseScale;
    }
}
