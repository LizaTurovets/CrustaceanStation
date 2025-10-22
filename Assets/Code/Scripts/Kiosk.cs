using System.Collections;
using UnityEngine;
public class Kiosk : MonoBehaviour
{
    private CrabSelector crabSelector;
    private GameObject prevCrab;
    private GameObject currentCrab;

    [SerializeField] private GameObject crabParentObject;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Clock clock;

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
            currentCrab.GetComponent<CrabController>().MakeDisappear();
            // TODO: Logic for selecting train


            //wait a moment
            StartCoroutine(WaitAMoment());
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

    public void OpenKiosk()
    {
        isOpen = true;
    }
    private IEnumerator WaitAMoment()
    {
        yield return new WaitForSeconds(3f);

        prevCrab = currentCrab;
        Destroy(currentCrab);
        SummonCrab();
    }

}
