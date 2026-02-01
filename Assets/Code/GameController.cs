using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private RoundTimer roundTimer;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private DollController dollController;
    [SerializeField] private float lightFadeDuration = 1f;
    [SerializeField] private SpriteRenderer lightBeam;
    [SerializeField] private Light2D pointLight;


    void Awake()
    {
        DollController.OnTutorialCompleted += TutorialCompleted;
    }

    private void TutorialCompleted()
    {
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
