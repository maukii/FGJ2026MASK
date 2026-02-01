using UnityEngine;

public class EnableKick : MonoBehaviour
{
    [SerializeField] Boot boot;

    void Start()
    {
        boot.canKick = true;
    }
}
