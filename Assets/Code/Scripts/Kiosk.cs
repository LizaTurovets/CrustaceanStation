using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Kiosk : MonoBehaviour
{
    private CrabSelector crabSelector;
    private GameObject currentCrab;
    [SerializeField] private Clock clock;

    [SerializeField] private GameObject crabParentObject; // in scene hierarchy: canvas > crabs
    [SerializeField] private GameObject ticketParentObject;
    [SerializeField] private GameObject canvas;

    [SerializeField] private TextMeshProUGUI coinCountText;
    [SerializeField] private Slider ratingsSlider;
    private bool isOpen = false;

    private int crabsToday = 0;
    private int wrongCrabs = 0;
    private float rating = 1;

    private void Awake()
    {
        ratingsSlider.value = 1;
        crabSelector = GetComponent<CrabSelector>();
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void SummonCrab()
    {
        currentCrab = Instantiate(crabSelector.ChooseCrab(), crabParentObject.transform);
        currentCrab.GetComponent<CrabController>().SetCanvas(canvas.GetComponent<Canvas>());
        currentCrab.GetComponent<CrabController>().SetCrabSelector(crabSelector);
        currentCrab.GetComponent<CrabController>().SetClock(clock);
        currentCrab.GetComponent<CrabController>().SetTicketAndIDParentObject(ticketParentObject);
        currentCrab.GetComponent<CrabController>().MakeAppear();
    }
    public void OnApprove()
    {
        if (!isOpen) return;

        bool trainExists = false;
        if (clock.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID())) 
        {
            trainExists = true;
        }

        clock.SetTrainsClickable(true);

        if (!currentCrab.GetComponent<CrabController>().IsValid() || !trainExists)
        {
            wrongCrabs++;
        }
        
    }

    public void OnReject()
    {
        if (!isOpen) return;

        bool trainExists = false;
        if (clock.CheckTrainIDValidity(currentCrab.GetComponent<CrabController>().GetTrainID()))
        {
            trainExists = true;
        }

        if (currentCrab.GetComponent<CrabController>().IsValid() && trainExists)
        {
            wrongCrabs++;
        }
        
        DisappearCrab();
    }

    public void GivePlayerCoins(int newCoins)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + (int)(newCoins * rating));
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void DisappearCrab()
    {
        UpdateRating();

        currentCrab.GetComponent<CrabController>().MakeDisappear();
        StartCoroutine(WaitAMoment());
    }

    private void UpdateRating()
    {
        crabsToday++;
        rating = (crabsToday - wrongCrabs) / (float)crabsToday;
        ratingsSlider.value = rating;
    }

    public void OpenKiosk()
    {
        isOpen = true;
    }
    private IEnumerator WaitAMoment()
    {
        yield return new WaitForSeconds(3f);

        Destroy(currentCrab);
        SummonCrab();
    }

}
