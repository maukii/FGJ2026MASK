using UnityEngine;

public class AudioResponse : MonoBehaviour
{
    [SerializeField] private AudioClip rightDollAudio;
    [SerializeField] private AudioClip wrongDollAudio;

    void Awake()
    {
        DollController.OnCorrectDollKicked += OnCorrectDollKicked;
        DollController.OnWrongDollKicked += OnWrongDollKicked;
    }

    private void OnWrongDollKicked()
    {
        Invoke(nameof(DelayedWrongAudio), 0.5f);
    }

    private void DelayedRightAudio()
    {
        AudioSource.PlayClipAtPoint(rightDollAudio, Vector3.zero);
    }

    private void OnCorrectDollKicked()
    {
        Invoke(nameof(DelayedRightAudio), 0.5f);
    }

    private void DelayedWrongAudio()
    {
        AudioSource.PlayClipAtPoint(wrongDollAudio, Vector3.zero);
    }
}
