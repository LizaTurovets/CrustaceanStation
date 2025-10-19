using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    public void quitGame() {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
