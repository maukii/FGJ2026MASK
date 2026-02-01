using DG.Tweening;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float smallPunchScale = 0.1f;
    [SerializeField] private float smallPunchDuration = 0.1f;
    [SerializeField] private float bigPunchScale = 0.35f;
    [SerializeField] private float bigPunchDuration = 0.35f;


    private void Awake()
    {
        Boot.OnBootMoved += ShakeSmall;
        Boot.OnDollKicked += ShakeBig;
    }

    private void ShakeSmall(int dollIndex)
    {
        cameraTransform.DOPunchPosition(Vector3.one * smallPunchScale, smallPunchDuration);
        cameraTransform.DOPunchRotation(Vector3.forward * smallPunchScale, smallPunchDuration);
    }

    private void ShakeBig(int dollIndex)
    {
        cameraTransform.DOPunchPosition(Vector3.one * bigPunchScale, bigPunchDuration);
        cameraTransform.DOPunchRotation(Vector3.forward * bigPunchScale, bigPunchDuration);
    }

    void OnDestroy()
    {
        Boot.OnBootMoved -= ShakeSmall;
        Boot.OnDollKicked -= ShakeBig;
    }
}
