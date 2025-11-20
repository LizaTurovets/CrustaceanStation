using UnityEngine;
using UnityEngine.UIElements;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject backgroundDisplay;
    public void OnReturn()
    {
        gameObject.SetActive(false);
        backgroundDisplay.SetActive(false);
    }

}
