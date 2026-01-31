using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private GameObject timerUI;
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private int startTimeSeconds = 180;

    public static event Action OnTimerRanOut;

    private bool isRunning;
    private int remainingSeconds;


    private void Awake() => ResetTimer();

    private void Start()
    {
        timerUI.transform.localScale = Vector3.zero;
    }

    public void ShowAndStart()
    {
        timerUI.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(StartTimer);
    }

    private void StartTimer()
    {
        if (isRunning)
            return;

        isRunning = true;
        InvokeRepeating(nameof(Tick), 1f, 1f);
    }

    public void StopTimer()
    {
        if (!isRunning)
            return;

        isRunning = false;
        CancelInvoke(nameof(Tick));
    }

    public void ResetTimer()
    {
        StopTimer();
        remainingSeconds = Mathf.Max(0, startTimeSeconds);
        UpdateDisplay();
    }

    private void Tick()
    {
        remainingSeconds--;

        if (remainingSeconds <= 0)
        {
            remainingSeconds = 0;
            UpdateDisplay();
            StopTimer();
            OnTimerRanOut?.Invoke();
            return;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        int minutes = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;
        timerLabel.SetText($"{minutes:00}:{seconds:00}");
    }
}
