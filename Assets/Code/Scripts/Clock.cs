using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    // CLOCK INFO
    // day is 8am to 8pm, each hour is 2 minutes
    private int startTime = 0;
    private int endTime = 12;
    private int currentTime;
    private float rotateAmount = 30.0f;
    [SerializeField] private float rotDuration = 1.0f;


    // TRAIN INFO
    [System.Serializable]
    public struct TrainInfoPair
    {
        public Train trainInfo;
        public bool chosen;
    }
    [SerializeField] private GameObject trainPrefab;
    [SerializeField] private TrainInfoPair[] trainInfos;
    private int numTrains = 4;                                                      // TODO: Add more train ScriptableObjects
    private GameObject[] trains;
    private List<TrainController> currentTrains = new List<TrainController>();

    [SerializeField] private GameObject clockHand;


    // CRABS
    [SerializeField] private Kiosk kiosk;


    private void Awake()
    {
        currentTime = startTime;
        StartCoroutine(TimeItself());
    }

    private void AddTrains()
    {
        trains = new GameObject[numTrains];
        for (int i = 0; i < numTrains; i++)
        {
            GameObject train = Instantiate(trainPrefab);

            int idx = Random.Range(0, trainInfos.Length);
            if (trainInfos[idx].chosen) {
                while (trainInfos[idx].chosen)
                {
                    idx = Random.Range(0, trainInfos.Length);
                }
            }
            train.GetComponent<TrainController>().SetTrainInfo(trainInfos[idx].trainInfo);
            trainInfos[idx].chosen = true;

            trains[i] = train;
        }
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

    // CLOCK ANIMATION
    private IEnumerator TimeItself()
    {
        while (currentTime < endTime)
        {
            yield return new WaitForSeconds(5f);

            // rotate clock hand
            yield return RotateHand();

            // check train arrivals and departures
            if (currentTime == startTime)
            {
                AddTrains();
            }
            else
            {
                if (currentTime == startTime + 1)
                {
                    kiosk.SummonCrab();
                }

                foreach (GameObject train in trains)
                {
                    TrainController controller = train.GetComponent<TrainController>();
                    if (controller != null)
                    {
                        if (currentTime == controller.GetArrivalTime())
                        {
                            controller.arriveTrain();
                            currentTrains.Add(controller);
                        }
                        else if (currentTime == controller.GetDepartureTime())
                        {
                            controller.departTrain();
                            currentTrains.Remove(controller);
                            Debug.Log(currentTrains.Count);
                        }
                    }

                }
            }
            
            currentTime++;
        }
    }

    private IEnumerator RotateHand()
    {
        Quaternion startRot = clockHand.transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, -rotateAmount, 0);

        float elapsed = 0f;
        while (elapsed < rotDuration)
        {
            clockHand.transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / rotDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        clockHand.transform.rotation = endRot;
    }


    // FOR DEBUGGING PURPOSES
    [ContextMenu("DepartTrain")]
    private void DepartTrains()
    {
        foreach (GameObject train in trains)
        {
            TrainController controller = train.GetComponent<TrainController>();
            if (controller != null)
            {
                controller.departTrain();
            }    
        }
    }

    [ContextMenu("ArriveTrain")]
    private void ArriveTrains()
    {
        foreach (GameObject train in trains)
        {
            TrainController controller = train.GetComponent<TrainController>();
            if (controller != null)
            {
                controller.arriveTrain();
            }    
        }
    }
}
