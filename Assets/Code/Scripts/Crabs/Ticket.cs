using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Ticket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    private string crabName;

    [SerializeField] private TextMeshProUGUI trainIDText;
    private string trainID = "G5";

    private string[] letters = { "A", "B", "C", "D", "E", "F" };
    private int[] numbers = { 1, 2, 3, 4 };

    public void SetName(string newName)
    {
        crabName = newName;
        nameText.text = crabName;
    }

    public void SetTrainID(string newTrainID)
    {
        trainID = newTrainID;
        trainIDText.text = trainID;
    }

    public string GetRandomTrainID()
    {
        return letters[Random.Range(0, letters.Length)] + numbers[Random.Range(0, numbers.Length)].ToString();
    }

    public string GetTrainID()
    {
        return trainID;
    }
}
