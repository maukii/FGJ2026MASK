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


    void Awake()
    {
        DollController.OnTutorialCompleted += TutorialCompleted;
    }

    private void TutorialCompleted()
    {
        DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, 0f, lightFadeDuration).SetEase(Ease.OutQuad);
        gameTimer.ShowAndStart();
        roundTimer.ShowAndStart();
    }

    private void Start()
    {
        globalLight.intensity = 1f;
        dollController.SpawnTutorialRound();
    }
}
