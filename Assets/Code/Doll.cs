using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Doll : MonoBehaviour
{
    [SerializeField] private Transform shakeTransform;
    [SerializeField] private ParticleSystem sweatParticle;
    [SerializeField] private List<GameObject> heads = new List<GameObject>();
    [SerializeField] private List<GameObject> masks = new List<GameObject>();

    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeStrength = 0.1f;

    public bool IsImposter { get; private set; }
    public int DollIndex { get; private set; }


    public void Initialize(int dollIndex, int headIndex, bool isImposter)
    {
        DollIndex = dollIndex;
        
        if (Boot.Instance.BootHoverIndex == dollIndex)
        {
            shakeTransform.DOShakePosition(shakeDuration, shakeStrength).SetLoops(-1);
            sweatParticle.Play();
        }
        else
        {
            DOTween.Kill(shakeTransform);
            sweatParticle.Stop();
        }

        IsImposter = isImposter;

        DisableAllHeads();
        DisableAllMasks();

        int finalHeadIndex = headIndex;
        if (isImposter)
        {
            do
            {
                finalHeadIndex = GetRandomHeadIndex();
            }
            while (finalHeadIndex == headIndex);
        }

        heads[finalHeadIndex].SetActive(true);
        masks[GetRandomMaskIndex()].SetActive(true);
    }

    private void Awake()
    {
        Boot.OnBootMoved += OnBootMoved;
    }

    void OnDestroy()
    {
        Boot.OnBootMoved -= OnBootMoved;
    }

    private void OnBootMoved(int slotIndex)
    {   
        DOTween.Kill(shakeTransform);
        sweatParticle.Stop();
        if (slotIndex == DollIndex)
        {
            sweatParticle.Play();
            shakeTransform.DOShakePosition(shakeDuration, shakeStrength).SetLoops(-1);
        }
    }

    private void DisableAllMasks()
    {
        foreach (var mask in masks)
        {
            mask.SetActive(false);
        }
    }

    private void DisableAllHeads()
    {
        foreach (var head in masks)
        {
            head.SetActive(false);
        }
    }

    private int GetRandomHeadIndex()
    {
        return Random.Range(0, heads.Count);
    }

    private int GetRandomMaskIndex()
    {
        return Random.Range(0, masks.Count);
    }
}
