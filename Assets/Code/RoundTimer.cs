using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RoundTimer : MonoBehaviour
{
    public static event System.Action OnRoundTimerRanOut;

    [SerializeField] private GameObject timerUI;
    [SerializeField] private Slider roundTimerSlider;
    [SerializeField] private Image fillImage;
    [SerializeField] private int roundTimeInSeconds = 10;
    [SerializeField] private float dangerThreshold = 0.3f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color dangerColor = Color.red;
    [SerializeField] private float dangerPulseScale = 1.25f;
    [SerializeField] private float dangerPulseDuration = 0.2f;
    [SerializeField] private Ease dangerPulseEase = Ease.InOutSine;
    [SerializeField] private float resetPunchScale = 0.15f;

    private Tween timerTween;
    private Tween dangerPulseTween;
    private Tween dangerPulseTween2;
    private bool tutorialCompleted = false;


    private void Awake()
    {
        DollController.OnCorrectDollKicked += ResetTimer;
        DollController.OnWrongDollKicked += ResetTimer;
    }

    private void Start()
    {
        roundTimerSlider.minValue = 0;
        roundTimerSlider.maxValue = roundTimeInSeconds;
        fillImage.color = normalColor;

        timerUI.transform.localScale = Vector3.zero;
    }

    public void ShowAndStart()
    {
        tutorialCompleted = true;
        timerUI.transform.DOScale(Vector3.one, 0.5f).SetDelay(0.25f).SetEase(Ease.OutBack).OnComplete(StartTimer);
    }

    public void StartTimer()
    {
        KillTweens();

        roundTimerSlider.value = roundTimeInSeconds;

        timerTween = DOTween.To(() => roundTimerSlider.value, x => roundTimerSlider.value = x, 0f, roundTimeInSeconds)
            .SetEase(Ease.Linear)
            .OnUpdate(CheckDangerZone)
            .OnComplete(TimerExpired);
    }

    private void TimerExpired()
    {
        roundTimerSlider.value = 0f;
        OnRoundTimerRanOut?.Invoke();
    }

    private void ResetTimer()
    {
        if (!tutorialCompleted) return;

        KillTweens();

        roundTimerSlider.value = roundTimeInSeconds;

        roundTimerSlider.transform.localScale = Vector3.one;
        roundTimerSlider.transform.localRotation = Quaternion.identity;

        fillImage.color = Color.white;
        fillImage.transform.DOPunchScale(Vector3.one * resetPunchScale, 1f, 8, 1f);
        fillImage.transform.DOPunchRotation(Vector3.forward, 1f);

        StartTimer();
    }

    private void CheckDangerZone()
    {
        float normalized = roundTimerSlider.value / roundTimeInSeconds;

        if (normalized <= dangerThreshold && dangerPulseTween == null)
        {
            fillImage.DOColor(dangerColor, 0.25f);

            dangerPulseTween = roundTimerSlider.transform
                .DOScale(dangerPulseScale, dangerPulseDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(dangerPulseEase);

            dangerPulseTween2 = roundTimerSlider.transform
                .DOPunchRotation(Vector3.forward * dangerPulseScale, dangerPulseDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(dangerPulseEase);
        }
    }

    private void KillTweens()
    {
        timerTween?.Kill();
        dangerPulseTween?.Kill();
        dangerPulseTween2?.Kill();
        dangerPulseTween = null;
        dangerPulseTween2 = null;
    }

    private void OnDestroy()
    {
        DollController.OnCorrectDollKicked -= ResetTimer;
        DollController.OnWrongDollKicked -= ResetTimer;
        KillTweens();
    }
}
