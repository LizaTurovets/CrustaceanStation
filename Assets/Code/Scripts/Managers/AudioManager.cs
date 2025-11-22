using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Volume");
        audioSource.Play();
    }

    public void Play()
    {
        audioSource.UnPause();
    }
}
