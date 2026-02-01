using UnityEngine;
using TMPro;
using System;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private GameObject timerUI;
    [SerializeField] private TextMeshProUGUI timerLabel;
    [SerializeField] private Image clockGraphic;
    [SerializeField] private int startTimeSeconds = 180;

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
            SceneManager.LoadScene("GameOver");
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        timerLabel.transform.DOPunchScale(Vector3.one * 0.5f, 0.1f);
        timerLabel.transform.DOPunchRotation(Vector3.one * 1.05f, 0.1f);

        clockGraphic.transform.DOPunchScale(Vector3.one * 0.5f, 0.1f).SetDelay(0.5f);
        clockGraphic.transform.DOPunchRotation(Vector3.one * 1.05f, 0.1f).SetDelay(0.5f);

        int minutes = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;
        timerLabel.SetText($"{minutes:00}:{seconds:00}");
    }
}
