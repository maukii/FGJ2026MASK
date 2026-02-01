using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickPunch : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] float punchScale = 0.15f;
    [SerializeField] float duration = 0.2f;
    [SerializeField] int vibrato = 8;
    [SerializeField] float elasticity = 0.5f;


    RectTransform rect;
    Vector3 baseScale;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        baseScale = rect.localScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        KillTween(true);
        rect.localScale = baseScale;

        rect.DOPunchScale(
            Vector3.one * punchScale,
            duration,
            vibrato,
            elasticity
        );

        Vector3 direction = Random.value > 0.5f ? Vector3.forward : Vector3.back;
        rect.DOPunchRotation(direction * 1.5f, duration);
    }

    private void KillTween(bool complete = false)
    {
        DOTween.Kill(rect, complete);
    }
}
