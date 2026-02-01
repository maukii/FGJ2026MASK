using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    public void OnPlayAgainClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
