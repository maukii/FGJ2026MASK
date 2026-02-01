using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PunchOnMouseEnter : MonoBehaviour, IPointerEnterHandler
{
    RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DOTween.Kill(rectTransform, true);

        rectTransform.DOPunchPosition(Vector3.one * 0.25f, 0.1f);
        rectTransform.DOPunchRotation((Random.value > 0.5f ? Vector3.forward : Vector3.back) * 8f, .15f);
        rectTransform.DOPunchScale(Vector3.one * 0.25f, 0.2f);
    }
}
