using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

    [SerializeField] private Clock clock;

    // prefabs
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject goalRating;
    [SerializeField] private GameObject goalCrabCount;
    [SerializeField] private GameObject summaryMenu;

    // UI background
    [SerializeField] private GameObject transparentOverlay;

    // Player goals for the day
    private bool isRating = false;
    private bool isCrabCount = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        goalRating.SetActive(false);
        goalCrabCount.SetActive(false);
        summaryMenu.SetActive(false);

        StartCoroutine(ShowGoalForTheDay());
    }

    public void OnPause()
    {
        // show overlay background
        transparentOverlay.SetActive(true);

        // stop clock & crabs & trains
        Time.timeScale = 0f;

        // show goal on pause screen?
    }

    public void OnResume()
    {
        // hide overlay background
        transparentOverlay.SetActive(false);

        // start clock & crabs & trains
        Time.timeScale = 1f;
    }

    public void OnStartLevel()
    {
        // start the clock
        clock.BeginDay();
    }

    public void ShowStatsForTheDay()
    {
        // show prefab
        transparentOverlay.SetActive(true);
        summaryMenu.SetActive(true);
    }

    public void OnShop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void OnMenu()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void OnNewDay()
    {
        SceneManager.LoadScene("Temp");
    }


    private IEnumerator ShowGoalForTheDay()
    {
        transparentOverlay.SetActive(true);

        // show prefab
        if (Random.Range(0, 10) > 4)
        {
            isRating = true;
            goalRating.SetActive(true);
        }
        else
        {
            isCrabCount = true;
            goalCrabCount.SetActive(true);
        }

        yield return new WaitForSeconds(3.5f);

        // hide prefab
        if (isRating)
        {
            goalRating.SetActive(false);
        }
        else
        {
            goalCrabCount.SetActive(false);
        }

        transparentOverlay.SetActive(false);

        OnStartLevel();
    }

}
