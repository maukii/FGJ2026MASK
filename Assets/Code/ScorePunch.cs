using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePunch : MonoBehaviour
{
    [SerializeField] private Slider timerSlider;
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private float punchScale = 0.2f;
    [SerializeField] private float punchDuration = 0.25f;
    [SerializeField] private float punchRotation = 8f;

    Tween scaleTween;
    Tween rotationTween;
    private int score = 0;
    private bool tutorialCompleted = false;


    private void Awake()
    {
        DollController.OnCorrectDollKicked += OnCorrectDollKicked;
        DollController.OnTutorialCompleted += OnTutorialCompleted;
    }

    private void OnTutorialCompleted()
    {
        tutorialCompleted = true;
    }

    private void OnCorrectDollKicked()
    {
        if (!tutorialCompleted) return;

        score += Mathf.RoundToInt(timerSlider.value);

        scaleTween?.Kill(true);
        rotationTween?.Kill(true);

        scaleTween = transform.DOPunchScale(
            Vector3.one * punchScale,
            punchDuration,
            vibrato: 1,
            elasticity: 0.6f
        );

        rotationTween = transform.DOPunchRotation(
            new Vector3(0, 0, UnityEngine.Random.Range(-punchRotation, punchRotation)),
            punchDuration,
            vibrato: 1,
            elasticity: 0.5f
        );

        scoreLabel.SetText($"{score}");
        Score.lastScore = score;
    }

    private void Start()
    {
        scoreLabel.SetText("");
    }

    void OnDestroy()
    {
        DollController.OnCorrectDollKicked -= OnCorrectDollKicked;
        DollController.OnTutorialCompleted -= OnTutorialCompleted;
    }
}

public static class Score
{
    public static int lastScore;
}
