using System;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System.Collections.Generic;
using System.Data.Common;

public class TrainController : MonoBehaviour
{
    private bool isMoving = false;
    private bool isDeparting = false;
    private bool isArriving = false;
    [SerializeField] private RectTransform trainTransform;
 
    private Vector3 startingPosArrive; // where the train is before it moves into the station
    private Vector3 startingPosDepart; // also endPosArrive
    private Vector3 endPosDepart; // where the train goes to be completely offscreen
    private float speed = 600.0f;

    // IDS and INFO
    private Train trainInfo;

    [SerializeField] private TextMeshProUGUI text;
    private string[] trainIDLetters = { "A", "B", "C", "D", "E", "F" };
    private string trainID = "";

    private int coins = 0;

    // SELECTION
    [SerializeField] private GameObject cartParent;
    [SerializeField] private GameObject[] cartTypes;
    private List<TrainSelection> trainSelections = new List<TrainSelection>();
    private float cartPosStartingPoint = 0f;
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
        trainInfo = ScriptableObject.CreateInstance<Train>();
        trainInfo.arrivalTimeHour = newArrivalTime;
        trainInfo.departureTimeHour = newDepartureTime;
        trainInfo.trainID = newTrainID;
        Init();
    }

    public bool IsStartTime0()
    {
        return trainInfo.arrivalTimeHour == 0;
    }
    public void SetArrivalTime()
    {
        trainInfo.arrivalTimeHour = 0;
    }

    public void SetKiosk(Kiosk newKiosk)
    {
        kiosk = newKiosk;
    }

    public string GetID()
    {
        return trainID;
    }

    public void SetThisClickable(bool isClickable)
    {
        foreach (TrainSelection ts in trainSelections) {
            ts.SetThisClickable(isClickable);
        }
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
            x = 194;
            //text.rectTransform.anchoredPosition = new Vector2(223, -23);
            //alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(163, 28);

        }
        else if (trainInfo.trainID == 2)
        {
            x = 394;
            //text.rectTransform.anchoredPosition = new Vector2(440, -23);
            //alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(379, 28);

        }
        else if (trainInfo.trainID == 3)
        {
            x = 594;
            //text.rectTransform.anchoredPosition = new Vector2(657, -23);
            //alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(580, 28);
        }
        else if (trainInfo.trainID == 4)
        {
            x = 794;
            //text.rectTransform.anchoredPosition = new Vector2(874, -23);
            //alertObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(781, 28); 
        }

        startingPosArrive = new Vector3(x, 830, 0);
        startingPosDepart = new Vector3(x, -250, 0);
        endPosDepart = new Vector3(x, -1280, 0);
        trainTransform.anchoredPosition = startingPosArrive;

        trainID = trainIDLetters[UnityEngine.Random.Range(0, trainIDLetters.Length)] + trainInfo.trainID.ToString();
    }

    private void Awake()
    {
        text.text = "";
    }

    private void Start()
    {
        int numOfCarts = UnityEngine.Random.Range(1, 5);
        for (int i = 0; i < numOfCarts; i++)
        {
            // instantiate cart as child
            GameObject cart = Instantiate(cartTypes[UnityEngine.Random.Range(0, 3)], cartParent.transform);
            TrainSelection selection = cart.GetComponent<TrainSelection>();

            // figure out position
            if (i == 0)
            {
                // set position based on cart type
                float startingPos = selection.getStartingPos();
                cart.transform.localPosition = new Vector3(-30, startingPos, 0);

                // add to cartPosStartingPoint
                cartPosStartingPoint += startingPos + selection.getHeight();
            }
            else
            {
                // set position based on cartPosStartingPoint
                cart.transform.localPosition = new Vector3(-30, cartPosStartingPoint, 0);

                // add to cartPosStartingPoint
                cartPosStartingPoint += selection.getHeight();
            }

            // add trainSelection to list
            trainSelections.Add(selection);

            // set kiosk
            selection.SetKiosk(kiosk);
            selection.SetController(this);
        }
    }

    public void arriveTrain()
    {
        isMoving = true;
        isArriving = true;

        text.text = trainID;
    } 

    public void departTrain()
    {
        text.text = "";

        // give player coins
        kiosk.GivePlayerCoins(coins);

        // move train offscreen (down)
        isMoving = true;
        isDeparting = true;
    }

    public void AddToCrabsOnTrain(int ticketCost)
    {
        coins += ticketCost;
    }

    private void Update()
    {
        if (isMoving)
        {

            if (isArriving)
            {
                // move train down from offscreen
                var step = speed * Time.deltaTime;
                trainTransform.anchoredPosition = Vector3.MoveTowards(trainTransform.anchoredPosition, startingPosDepart, step);

                if (trainTransform.anchoredPosition.y <= startingPosDepart.y)
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
                trainTransform.anchoredPosition = Vector3.MoveTowards(trainTransform.anchoredPosition, endPosDepart, step);

                if (trainTransform.anchoredPosition.y <= endPosDepart.y)
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
