using System;
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
    [SerializeField] private Train trainInfo;

    [SerializeField] private TextMeshProUGUI text;
    private string[] trainIDLetters = { "A", "B", "C", "D", "E", "F" };
    private string trainID = "";

    // GETTERS
    public float GetArrivalTime()
    {
        return trainInfo.arrivalTimeHour;
    }

    public float GetDepartureTime()
    {
        return trainInfo.departureTimeHour;
    }

    public void SetTrainInfo(Train newInfo)
    {
        trainInfo = newInfo;
        Init();
    }

    public string GetID()
    {
        return trainID;
    }

    public void Init()
    {
        float x = 0;
        if (trainInfo.trainID == 1)
        {
            x = 1.5f;
            text.rectTransform.anchoredPosition = new Vector2(223, -23);
        }
        else if (trainInfo.trainID == 2)
        {
            x = 3.5f;
            text.rectTransform.anchoredPosition = new Vector2(440, -23);

        }
        else if (trainInfo.trainID == 3)
        {
            x = 5.5f;
            text.rectTransform.anchoredPosition = new Vector2(657, -23);
        }
        else if (trainInfo.trainID == 4)
        {
            x = 7.5f;
            text.rectTransform.anchoredPosition = new Vector2(874, -23);
        }

        startingPosArrive = new Vector3(x, 8.47f, 0);
        startingPosDepart = new Vector3(x, 2.5f, 0);
        endPosDepart = new Vector3(x, -7.0f, 0);
        trainTransform.position = startingPosArrive;

        trainID = trainIDLetters[UnityEngine.Random.Range(0, trainIDLetters.Length)] + trainInfo.trainID.ToString();
    }

	private void Awake()
	{
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


        // give player coins


        // move train offscreen (down)
        isMoving = true;
        isDeparting = true;
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
}
