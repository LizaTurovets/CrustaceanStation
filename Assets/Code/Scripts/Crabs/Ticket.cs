using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Ticket : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    private string crabName;

    [SerializeField] private TextMeshProUGUI trainIDText;
    private string trainID;

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
}
