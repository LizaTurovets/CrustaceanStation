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

    private int crabsToday = 0;
    private int wrong = 0;
    private int total = 0;

    [SerializeField] private RatingGoal ratingGoal;
    [SerializeField] private CrabCountGoal crabCountGoal;

    private int crabSpeed = 5;

    private void Awake()
    {
        crabSelector = GetComponent<CrabSelector>();
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
        SetCrabSpeedUpgrade();
    }

    public void SummonCrab()
    {
        currentCrab = Instantiate(crabSelector.ChooseCrab(), crabParentObject.transform);

        CrabController controller = currentCrab.GetComponent<CrabController>();
        controller.SetCanvas(canvas.GetComponent<Canvas>());
        controller.SetCrabSelector(crabSelector);
        controller.SetClock(clock);
        controller.SetTicketAndIDParentObject(ticketParentObject);
        controller.MakeAppear();
    }
    public void OnApprove()         // KNOWN BUG: if train is full but the ticket is correct, it's still seen as an error
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
            wrong++;
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
            wrong++;
        }

        DisappearCrab();
    }

    public bool IsCrabValid()
    {
        return currentCrab.GetComponent<CrabController>().IsValid();
    }

    public void DowngradedCart()
    {
        wrong++;
    }

    public void UpgradedCart()
    {
        total++;
    }

    public Cart.Type GetCurrentCrabTicket()
    {
        return currentCrab.GetComponent<CrabController>().GetTicketType();
    }

    public void GivePlayerCoins(int newCoins)
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + (int)(newCoins * ratingGoal.GetRating()));
        coinCountText.text = PlayerPrefs.GetInt("coins").ToString();
    }

    public void DisappearCrab()
    {
        crabsToday++;
        total++;

        UpdateRating();

        crabCountGoal.IncrementGoal(crabsToday);

        currentCrab.GetComponent<CrabController>().MakeDisappear();
        StartCoroutine(WaitAMoment());
    }

    private void UpdateRating()
    {
        ratingGoal.UpdateRating((total - wrong) / (float)total);
    }

    public void OpenKiosk()
    {
        isOpen = true;
    }

    public void CloseKiosk()
    {
        Debug.Log("crabs total: " + crabsToday);
        isOpen = false;
        currentCrab.GetComponent<CrabController>().MakeDisappear();
    }

    private IEnumerator WaitAMoment()
    {
        yield return new WaitForSeconds(crabSpeed);

        Destroy(currentCrab);

        if (isOpen)
        {
            SummonCrab();
        }
    }

    public void SetCrabSpeedUpgrade()
    {
        int dropRate = PlayerPrefs.GetInt("crabDropRate");
        if (dropRate == 0)
        {
            crabSpeed = 5;
        }
        else if (dropRate == 1)
        {
            crabSpeed = 4;
        }
        else if (dropRate == 2)
        {
            crabSpeed = 3;
        }
        else if (dropRate == 3)
        {
            crabSpeed = 2;
        }

    }

    public int GetTotalCrabs()
    {
        return crabsToday;
    }

}
