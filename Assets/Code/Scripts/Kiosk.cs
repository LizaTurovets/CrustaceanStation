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

    private bool isOpen = false;

    private void Awake()
    {
        crabSelector = GetComponent<CrabSelector>();
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
            

        if (currentCrab.GetComponent<CrabController>().IsValid() && trainExists)               // COMPLETELY VALID
        {
            //currentCrab.GetComponent<CrabController>().MakeDisappear();
            // TODO: Logic for selecting train
            clock.SetTrainsClickable(true);

            //wait a moment
            //StartCoroutine(WaitAMoment());
        }
        else                                                                    // FORGERY!
        {
            // consequences?
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

        if (!currentCrab.GetComponent<CrabController>().IsValid() || !trainExists)
        {
            currentCrab.GetComponent<CrabController>().MakeDisappear();
            // TODO: crab make sad sound

            //wait a moment
            StartCoroutine(WaitAMoment());
        }
        else
        {
            // consequences
        }
    }

    public void GivePlayerCoins(int newCoins)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + newCoins);
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void DisappearCrab()
    {
        currentCrab.GetComponent<CrabController>().MakeDisappear();
        StartCoroutine(WaitAMoment());
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
