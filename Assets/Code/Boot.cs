using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Boot : MonoBehaviour
{
    public static Boot Instance { get; private set; }

    [SerializeField] private AudioClip bootMoveAudio;
    [SerializeField] private Transform bootParent;
    [SerializeField] private Animator bootAnimator;
    [SerializeField] private Animator beltAnimator;
    [SerializeField] private Transform spotlight;
    [SerializeField] private List<Transform> kickPositions = new List<Transform>();
    [SerializeField] private List<Transform> lightPositions = new List<Transform>();
    [SerializeField] private float slideDuration = 0.25f;
    [SerializeField] private Ease slideEase = Ease.InOutQuad;

    [SerializeField] private float failShakeDuration = 0.1f;
    [SerializeField] private float failShakeStrength = 0.25f;

    public static event Action<int> OnDollKicked;
    public static event Action OnKickAnimationFinished;
    public static event Action<int> OnBootMoved;

    public int BootHoverIndex => bootPositionIndex;
    private int bootPositionIndex = 0;
    private Tween tween;
    public bool canKick = false;


    private void Awake()
    {
        Instance = this;
        DollController.OnRerollCompleted += RerollCompleted;
        RoundTimer.OnRoundTimerRanOut += OnRoundTimeEnded;
    }

    private void OnRoundTimeEnded()
    {
        canKick = false;
    }

    private void RerollCompleted()
    {
        canKick = true;
    }

    private void Start()
    {
        bootParent.position = kickPositions[bootPositionIndex].position;
        spotlight.position = new Vector3(lightPositions[bootPositionIndex].position.x, spotlight.position.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveRight();

        if (Input.GetKeyDown(KeyCode.Space))
            TryKick();
    }

    public void TryKick()
    {
        if (!canKick) return;

        bootAnimator.SetTrigger("Kick");
        canKick = false;
    }

    public void OnKick()
    {
        OnDollKicked?.Invoke(bootPositionIndex);
    }

    public void OnKickAnimationCompleted()
    {
        if (beltAnimator != null) beltAnimator.SetTrigger("Roll");
        OnKickAnimationFinished?.Invoke();
    }

    public void MoveLeft()
    {
        if (bootPositionIndex <= 0)
        {
            AnimateFail();
            return;
        }

        AudioSource.PlayClipAtPoint(bootMoveAudio, Vector3.zero);
        bootPositionIndex--;
        bootPositionIndex = Mathf.Clamp(bootPositionIndex, 0, kickPositions.Count-1);
        OnBootMoved?.Invoke(bootPositionIndex);

        AnimateToIndex();
    }

    public void MoveRight()
    {
        if (bootPositionIndex >= kickPositions.Count-1)
        {
            AnimateFail();
            return;
        }

        AudioSource.PlayClipAtPoint(bootMoveAudio, Vector3.zero);
        bootPositionIndex++;
        bootPositionIndex = Mathf.Clamp(bootPositionIndex, 0, kickPositions.Count-1);
        OnBootMoved?.Invoke(bootPositionIndex);

        AnimateToIndex();
    }

    private void AnimateFail()
    {
        KillTween(true);
        tween = transform.DOShakePosition(failShakeDuration, failShakeStrength);
    }

    private void AnimateToIndex()
    {
        KillTween(true);
        tween = bootParent.DOMoveX(kickPositions[bootPositionIndex].position.x, slideDuration).SetEase(slideEase);
        spotlight.DOMoveX(lightPositions[bootPositionIndex].position.x, slideDuration).SetEase(slideEase);
    }

    private void KillTween(bool complete = false)
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill(complete);
        }
    }

    void OnDestroy()
    {
        DollController.OnRerollCompleted -= RerollCompleted;
        RoundTimer.OnRoundTimerRanOut -= OnRoundTimeEnded;
    }
}
