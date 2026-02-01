using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private AudioClip explostionAudio;
    [SerializeField] private Boot boot;
    [SerializeField] private Doll startGameDoll;
    [SerializeField] private Doll creditsDoll;
    [SerializeField] private Doll dollPrefab;
    [SerializeField] private ParticleSystem dollExplosionParticle;
    [SerializeField] private float particleOffset = 2.5f;
    [SerializeField] private GameObject creditsView;
    [SerializeField] private Transform creditsDollTargetPos;

    private bool loadGameplay = false;
    private bool showCredits = false;


    private void Awake()
    {
        Boot.OnDollKicked += OnDollKicked;
        Boot.OnKickAnimationFinished += OnKickFinished;
    }

    private void OnKickFinished()
    {
        if (loadGameplay)
        {
            loadGameplay = false;
            SceneManager.LoadScene("Gameplay");
        }
        if (showCredits)
        {
            showCredits = false;
            creditsView.SetActive(true);
            Doll doll = Instantiate(dollPrefab, creditsDollTargetPos.position, Quaternion.identity);
            doll.Initialize(1, Random.Range(0, 8), false, true);
        }

        boot.canKick = true;
    }

    private void OnDollKicked(int dollIndex)
    {
        if (dollIndex == 0)
        {
            loadGameplay = true;
            Vector3 particlePosition = new Vector3(startGameDoll.transform.position.x, startGameDoll.transform.position.y + particleOffset, -1.75f);
            Instantiate(dollExplosionParticle, particlePosition, Quaternion.identity);
            Destroy(startGameDoll.gameObject);
        }
        else
        {
            showCredits = true;
            Vector3 particlePosition = new Vector3(creditsDoll.transform.position.x, creditsDoll.transform.position.y + particleOffset, -1.75f);
            Instantiate(dollExplosionParticle, particlePosition, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explostionAudio, Vector3.zero);
            Destroy(creditsDoll.gameObject);
        }
    }

    private void Start()
    {
        boot.canKick = true;
        startGameDoll.Initialize(0, Random.Range(0, 8), false, true);
        creditsDoll.Initialize(1, Random.Range(0, 8), false, true);
    }
}
