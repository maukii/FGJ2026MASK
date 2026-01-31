using UnityEngine;

public class Boot : MonoBehaviour
{
    [SerializeField] private float bootCooldown = 2f;

    private float bootCooldownTimer;


    private void Update()
    {
        bootCooldownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryToBoot();
        }
    }

    public void OnBootButtonClicked() => TryToBoot();

    private void TryToBoot()
    {
        if (!CanGiveBoot()) return;

        GiveBoot();
    }

    private bool CanGiveBoot() => bootCooldownTimer <= 0f;

    private void GiveBoot()
    {
        bootCooldownTimer = bootCooldown;
        Debug.Log("BOOT");
    }
}
