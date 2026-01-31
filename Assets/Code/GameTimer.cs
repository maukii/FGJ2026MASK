using UnityEngine;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private int startTimeSeconds = 180;

    public static event Action OnTimerRanOut;

    private bool isRunning;
    private int remainingSeconds;


    private void Awake() => ResetTimer();

    public void StartTimer()
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
