using System.Collections;
using UnityEngine;
public class Kiosk : MonoBehaviour
{
    private CrabSelector crabSelector;
    private GameObject prevCrab;
    private GameObject currentCrab;

    [SerializeField] private GameObject crabParentObject;
    [SerializeField] private GameObject canvas;

    private void Awake()
    {
        crabSelector = GetComponent<CrabSelector>();
    }

    public void SummonCrab()
    {
        currentCrab = Instantiate(crabSelector.ChooseCrab(), crabParentObject.transform);
        currentCrab.GetComponent<CrabController>().SetCanvas(canvas.GetComponent<Canvas>());
        currentCrab.GetComponent<CrabController>().SetCrabSelector(crabSelector);
        currentCrab.GetComponent<CrabController>().MakeAppear();
    }
    public void OnApprove()
    {
        if (currentCrab.GetComponent<CrabController>().IsValid())
        {
            currentCrab.GetComponent<CrabController>().MakeDisappear();
            // TODO: Logic for selecting train


            //wait a moment
            StartCoroutine(WaitAMoment());
        }
        else
        {
            // consequences?
        }
        
    }

    public void OnReject()
    {
        if (!currentCrab.GetComponent<CrabController>().IsValid())
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


    private IEnumerator WaitAMoment()
    {
        yield return new WaitForSeconds(3f);

        prevCrab = currentCrab;
        Destroy(currentCrab);
        SummonCrab();
    }

}
