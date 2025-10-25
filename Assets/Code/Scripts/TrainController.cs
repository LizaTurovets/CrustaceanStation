using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TrainController : MonoBehaviour
{
    private bool isMoving = false;
    private bool isDeparting = false;
    private bool isArriving = false;
    [SerializeField] private Transform trainTransform;

    private Vector3 startingPosArrive; // where the train is before it moves into the station
    private Vector3 startingPosDepart; // also endPosArrive
    private Vector3 endPosDepart; // where the train goes to be completely offscreen
    private float speed = 10.0f;

    // IDS and INFO
    private Train trainInfo;

    [SerializeField] private TextMeshProUGUI text;
    private string[] trainIDLetters = { "A", "B", "C", "D", "E", "F" };
    private string trainID = "";

    private int crabsOnTrain = 0;

    // SELECTION
    [SerializeField] private TrainSelection trainSelection;
    private Kiosk kiosk;

    // ALERT
    [SerializeField] private GameObject alertObject;
    private bool isAlerting;

    // GETTERS
    public float GetArrivalTime()
    {
        return trainInfo.arrivalTimeHour;
    }

    public float GetDepartureTime()
    {
        return trainInfo.departureTimeHour;
    }

    public void SetTrainInfo(int newArrivalTime, int newDepartureTime, int newTrainID)
    {
        trainInfo.arrivalTimeHour = newArrivalTime;
        trainInfo.departureTimeHour = newDepartureTime;
        trainInfo.trainID = newTrainID;
        Init();
    }

    public bool IsStartTime0()
    {
        return (trainInfo.arrivalTimeHour == 0);
    }
    public void SetArrivalTime()
    {
        trainInfo.arrivalTimeHour = 0;
    }

    public void SetKiosk(Kiosk newKiosk)
    {
        kiosk = newKiosk;
        trainSelection.SetKiosk(newKiosk);
    }

    public string GetID()
    {
        return trainID;
    }

    public void SetThisClickable(bool isClickable)
    {
        trainSelection.SetThisClickable(isClickable);
    }

    public void AboutToDepartAlert()
    {
        isAlerting = true;
        StartCoroutine(Alert());
    }
    public void Init()
    {
        float x = 0;
        if (trainInfo.trainID == 1)
        {
            x = 1.5f;
            text.rectTransform.anchoredPosition = new Vector2(223, -23);
            alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(163, 28);

        }
        else if (trainInfo.trainID == 2)
        {
            x = 3.5f;
            text.rectTransform.anchoredPosition = new Vector2(440, -23);
            alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(379, 28);

        }
        else if (trainInfo.trainID == 3)
        {
            x = 5.5f;
            text.rectTransform.anchoredPosition = new Vector2(657, -23);
            alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(580, 28);
        }
        else if (trainInfo.trainID == 4)
        {
            x = 7.5f;
            text.rectTransform.anchoredPosition = new Vector2(874, -23);
            alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(781, 28); 
        }

        startingPosArrive = new Vector3(x, 8.47f, 0);
        startingPosDepart = new Vector3(x, 2.5f, 0);
        endPosDepart = new Vector3(x, -7.0f, 0);
        trainTransform.position = startingPosArrive;

        trainID = trainIDLetters[UnityEngine.Random.Range(0, trainIDLetters.Length)] + trainInfo.trainID.ToString();
    }

    private void Awake()
    {
        trainInfo = ScriptableObject.CreateInstance<Train>();
        text.text = "";
    }

    public void arriveTrain()
    {
        isMoving = true;
        isArriving = true;

        text.text = trainID;

        // activate train picker?
    }

    public void departTrain()
    {
        text.text = "";
        // calculate capacity
        int coins = crabsOnTrain; // replace with economy * 1 + standard * 2 + premium * 3

        // give player coins
        kiosk.GivePlayerCoins(coins);

        // move train offscreen (down)
        isMoving = true;
        isDeparting = true;
    }

    public void AddToCrabsOnTrain()
    {
        crabsOnTrain++;
    }

    private void Update()
    {
        if (isMoving)
        {

            if (isArriving)
            {
                // move train down from offscreen
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, startingPosDepart, step);

                if (transform.position.y <= startingPosDepart.y)
                {
                    isArriving = false;
                    isMoving = false;
                }
            }
            else if (isDeparting)
            {
                isAlerting = false;

                // move train down from onscreen
                var step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, endPosDepart, step);

                if (transform.position.y <= endPosDepart.y)
                {
                    isDeparting = false;
                    isMoving = false;

                    Destroy(this);
                }

            }
        }
    }

    private IEnumerator Alert()
    {
        while (isAlerting)
        {
            alertObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            alertObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        

    }

}
