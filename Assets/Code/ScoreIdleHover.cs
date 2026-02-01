using DG.Tweening;
using UnityEngine;

public class ScoreIdleHover : MonoBehaviour
{
    [SerializeField] float positionAmplitude = 6f;
    [SerializeField] float rotationAmplitude = 3f;
    [SerializeField] float scaleAmplitude = 0.04f;
    [SerializeField] float duration = 2.5f;

    RectTransform rect;
    Sequence idleSequence;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        StartIdle();
    }

    private void StartIdle()
    {
        rect.DOAnchorPosY(positionAmplitude, duration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        rect.DORotate(new Vector3(0, 0, rotationAmplitude), duration + 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        rect.DOScale(1f + scaleAmplitude, duration + 0.6f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
}
