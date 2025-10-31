using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Analytics;

public class Clock : MonoBehaviour
{
    // CLOCK INFO
    // day is 8am to 8pm, each hour is 2 minutes
    private int startTime = 0;
    private int endTime = 24;
    private int currentTime = 0;
    private float rotateAmount = 30.0f;
    [SerializeField] private float rotDuration = 1.0f;

    private bool isPaused = false;


    // TRAIN INFO
    //[System.Serializable]
    /*public struct TrainInfoPair
    {
        public Train trainInfo;
        public bool chosen;
    }*/
    [SerializeField] private GameObject trainPrefab;
    [SerializeField] private GameObject trainParent;
    //[SerializeField] private TrainInfoPair[] trainInfos;
    private List<TrainController> currentTrains = new List<TrainController>();

    [SerializeField] private GameObject clockHand;


    // CRABS
    [SerializeField] private Kiosk kiosk;

    // TRAIN ARRAYS
    private List<GameObject>[] allTrains;

    private void Awake()
    {
        allTrains = new List<GameObject>[4];
        currentTime = startTime;
        StartCoroutine(TimeItself());
    }

    private void AddTrains()
    {
        bool startingTrain = false;    // is there a train that arrives before the first crab does?

        // goes through all of the trainIDs (lines) and fills it with disjoint trains
        for (int i = 0; i < allTrains.Length; i++)
        {
            allTrains[i] = AddTrainsToLine(i + 1);
            if (allTrains[i][0].GetComponent<TrainController>().IsStartTime0())
            {
                startingTrain = true;
            }
        }

        if (!startingTrain)
        {
            allTrains[Random.Range(0, allTrains.Length)][0].GetComponent<TrainController>().SetArrivalTime();
        }

    }

    private List<GameObject> AddTrainsToLine(int trainID)
    {
        List<GameObject> trainsInLine = new List<GameObject>();

        int arrival = Random.Range(0, 2);
        int timeSpent = Random.Range(3, 5);         // time spent at station
        int departure = arrival + timeSpent;

        GameObject train = Instantiate(trainPrefab, trainParent.transform);
        train.SetActive(true);
        train.GetComponent<TrainController>().SetTrainInfo(arrival, departure, trainID);
        train.GetComponent<TrainController>().SetKiosk(kiosk);
        trainsInLine.Add(train);

        int prevDeparture = departure;
        bool doneWithThisID = false;

        while (!doneWithThisID)                     // while there is still time for more trains
        {
            arrival = Random.Range(prevDeparture + 1, prevDeparture + 3);  // new start time (with some padding after previous train)
            timeSpent = Random.Range(3, 6);
            departure = arrival + timeSpent;

            if (departure > endTime)                // latest departure time, so we're done here
            {
                departure = endTime;
                doneWithThisID = true;
            }
            else if (departure >= (endTime - 3))    // not enough time for another train, so we're done here
            {
                doneWithThisID = true;
            }

            train = Instantiate(trainPrefab, trainParent.transform);
            train.SetActive(true);
            train.GetComponent<TrainController>().SetTrainInfo(arrival, departure, trainID);
            train.GetComponent<TrainController>().SetKiosk(kiosk);
            trainsInLine.Add(train);

            prevDeparture = departure;
        }

        return trainsInLine;
    }

    public string GetRandomCurrentTrainID()
    {
        return currentTrains[Random.Range(0, currentTrains.Count)].GetID();
    }

    public bool CheckTrainIDValidity(string id)
    {
        foreach (TrainController train in currentTrains)
        {
            if (id == train.GetID())
            {
                return true;
            }
        }
        return false;
    }

    public void SetTrainsClickable(bool allowClick)
    {
        foreach (TrainController train in currentTrains)
        {
            train.SetThisClickable(allowClick);
        }
    }

    // CLOCK ANIMATION
    private IEnumerator TimeItself()
    {
        while (currentTime < endTime)
        {

            if (currentTime == startTime)
            {
                AddTrains();
                CheckTrains();

                yield return WaitThenSummonCrabs();
            }
            else
            {
                CheckTrains();
            }

            yield return new WaitForSeconds(10f);

            // rotate clock hand
            yield return RotateHand();
            currentTime++;
        }
    }

    private void CheckTrains()
    {
        List<int> removeIndex = new List<int>();

        // checks for new arrivals and departures
        foreach (List<GameObject> trainID in allTrains)
        {
            for (int i = 0; i < trainID.Count; i++)
            {
                GameObject train = trainID[i];
                if (train == null) { continue; } // all trains have departed on this line, so we can skip it

                TrainController controller = train.GetComponent<TrainController>();
                if (controller != null)
                {
                    if (currentTime == controller.GetArrivalTime())
                    {
                        currentTrains.Add(controller);
                        controller.arriveTrain();
                    }
                    else if (currentTime == controller.GetDepartureTime())
                    {
                        currentTrains.Remove(controller);
                        removeIndex.Add(i);
                        controller.departTrain();
                    }
                    else if (currentTime == controller.GetDepartureTime() - 1)
                    {
                        controller.AboutToDepartAlert();
                    }
                }
            }

            foreach (int index in removeIndex)
            {
                trainID.RemoveAt(index);
            }
            removeIndex.Clear();
        }
    }

    private IEnumerator RotateHand()
    {
        Quaternion startRot = clockHand.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 0, -rotateAmount);

        float elapsed = 0f;
        while (elapsed < rotDuration)
        {
            clockHand.transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        clockHand.transform.rotation = endRot;
        
    }

    private IEnumerator WaitThenSummonCrabs()
    {
        yield return new WaitForSeconds(1f);
        kiosk.SummonCrab();
        kiosk.OpenKiosk();
    }
}
