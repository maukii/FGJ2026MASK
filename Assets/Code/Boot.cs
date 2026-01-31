using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Boot : MonoBehaviour
{
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

    private int bootPositionIndex = 0;
    private Tween tween;
    private bool canKick = false;


    private void Awake()
    {
        DollController.OnRerollCompleted += RerollCompleted;
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
        if (!canKick) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            MoveRight();

        if (Input.GetMouseButton(0))
            TryKick();
    }

    private void TryKick()
    {
        bootAnimator.SetTrigger("Kick");
        canKick = false;
    }

    public void OnKick()
    {
        // check which index was kicked
        // tell it to explode
        // event if it was correct or incorrect choise
        OnDollKicked?.Invoke(bootPositionIndex);
    }

    public void OnKickAnimationCompleted()
    {
        beltAnimator.SetTrigger("Roll");
        OnKickAnimationFinished?.Invoke();
    }

    private void MoveLeft()
    {
        if (bootPositionIndex <= 0)
        {
            AnimateFail();
            return;
        }

        bootPositionIndex--;
        bootPositionIndex = Mathf.Clamp(bootPositionIndex, 0, kickPositions.Count-1);
        AnimateToIndex();
    }

    private void MoveRight()
    {
        if (bootPositionIndex >= kickPositions.Count-1)
        {
            AnimateFail();
            return;
        }

        bootPositionIndex++;
        bootPositionIndex = Mathf.Clamp(bootPositionIndex, 0, kickPositions.Count-1);
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
}
