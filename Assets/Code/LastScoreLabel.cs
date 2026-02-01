using TMPro;
using UnityEngine;

public class LastScoreLabel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI lastScoreLabel;

    void Start()
    {
        lastScoreLabel.SetText($"{Score.lastScore}");
    }
}
