using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public event Action OnGameOver;

    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private RoundTimer roundTimer;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private DollController dollController;
    [SerializeField] private float lightFadeDuration = 1f;
    [SerializeField] private SpriteRenderer lightBeam;
    [SerializeField] private Light2D pointLight;
    [SerializeField] private List<Image> healthParentGraphics = new List<Image>();
    [SerializeField] private List<Image> healthGraphics = new List<Image>();

    private int currentHealth;
    private bool tutorialCompleted = false;


    void Awake()
    {
        currentHealth = 3;

        DollController.OnTutorialCompleted += TutorialCompleted;
        DollController.OnWrongDollKicked += LoseLife;
    }

    private void LoseLife()
    {
        if (!tutorialCompleted) return;

        currentHealth--;

        healthParentGraphics[currentHealth].transform.DOPunchScale(Vector3.one * 0.25f, 0.35f);
        healthParentGraphics[currentHealth].transform.DOPunchRotation(Vector3.forward, 0.25f);

        healthGraphics[currentHealth].transform.DOScale(0f, 0.25f).SetEase(Ease.OutExpo);
        healthGraphics[currentHealth].transform.DOLocalRotate(new Vector3(0f, 0f, -360f), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo);

        if (currentHealth <= 0)
        {
            OnGameOver?.Invoke();
        }
    }

    private void TutorialCompleted()
    {
        tutorialCompleted = true;

        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0f, lightFadeDuration).SetEase(Ease.OutQuad);
        DOTween.To(() => pointLight.intensity, x => pointLight.intensity = x, 1f, lightFadeDuration).SetEase(Ease.OutQuad);
        lightBeam.DOFade(30f, lightFadeDuration);
        gameTimer.ShowAndStart();
        roundTimer.ShowAndStart();
    }

    private void Start()
    {
        lightBeam.DOFade(0f, 0f);
        pointLight.intensity = 0f;
        globalLight.intensity = 1f;
        dollController.SpawnTutorialRound();
    }
}
