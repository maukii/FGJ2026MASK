using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DollController : MonoBehaviour
{
    public static event Action OnRerollCompleted;

    [SerializeField] private Doll dollPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Transform> targetPoints;
    [SerializeField] private List<Transform> exitPoints;

    [SerializeField] private float moveDuration = 0.35f;
    [SerializeField] private Ease moveEase = Ease.InOutQuint;
    [SerializeField] private float tiltAngle = 8f;
    [SerializeField] private float tiltDuration = 0.35f;
    [SerializeField] private float tiltBackDuration = 0.1f;
    [SerializeField] private Ease tiltEase = Ease.OutBounce;
    [SerializeField] private Ease tiltBackEase = Ease.OutBack;

    private List<Doll> currentDolls = new List<Doll>();


    private void Awake()
    {
        Boot.OnKickAnimationFinished += OnKickFinished;
        Boot.OnDollKicked += OnDollKicked;
    }

    private void OnDollKicked(int dollIndex)
    {
        Doll doll = currentDolls[dollIndex];
        Destroy(doll.gameObject);
    }

    private void OnKickFinished()
    {
        AnimateExit(currentDolls);
        currentDolls = SpawnDolls();
        AnimateEnter(currentDolls);
    }

    private void Start()
    {
        currentDolls = SpawnDolls();
        AnimateEnter(currentDolls);
    }

    private List<Doll> SpawnDolls()
    {
        List<Doll> dolls = new List<Doll>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            Doll doll = Instantiate(dollPrefab, spawnPoint.position, Quaternion.identity);
            dolls.Add(doll);
        }

        return dolls;
    }

    private void AnimateEnter(List<Doll> dolls)
    {
        for (int i = 0; i < dolls.Count; i++)
        {
            Doll doll = dolls[i];
            Transform t = doll.transform;

            t.DOMove(targetPoints[i].transform.position, moveDuration).SetEase(moveEase)
            .OnComplete(() => OnRerollCompleted?.Invoke());
            t.DOLocalRotate(new Vector3(0f, 0f, tiltAngle), tiltDuration).SetEase(tiltEase)
            .OnComplete(() => t.DOLocalRotate(Vector3.zero, tiltBackDuration).SetEase(tiltBackEase));
        }
    }

    private void AnimateExit(List<Doll> dolls)
    {
        for (int i = 0; i < dolls.Count; i++)
        {
            if (dolls[i] == null) continue;

            Doll doll = dolls[i];
            Transform t = doll.transform;

            t.DOMove(exitPoints[i].transform.position, moveDuration).SetEase(moveEase);
            t.DOLocalRotate(new Vector3(0f, 0f, tiltAngle), tiltDuration).SetEase(tiltEase)
            .OnComplete(() => t.DOLocalRotate(Vector3.zero, tiltBackDuration).SetEase(tiltBackEase));
        }

        StartCoroutine(DestroyOldDolls(dolls));
    }

    private IEnumerator DestroyOldDolls(List<Doll> dolls, float delay = 1f)
    {
        yield return new WaitForSeconds(delay);

        foreach (Doll doll in dolls)
        {
            if (doll == null) continue;

            Destroy(doll.gameObject);
        }
    } 
}
