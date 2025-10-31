using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    public void quitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    public void startGame()
    {
        SceneManager.LoadScene("Temp");
    }
}
