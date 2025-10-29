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

    [SerializeField] private GameObject blur;
    [SerializeField] private RectTransform rectTransform;
    private ID id;

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

	public void SetID(ID newID)
	{
        id = newID;
	}

	public string GetRandomTrainID()
    {
        return letters[Random.Range(0, letters.Length)] + numbers[Random.Range(0, numbers.Length)].ToString();
    }

    public string GetTrainID()
    {
        return trainID;
    }

    public void PushBack()
    {
        rectTransform.rotation = Quaternion.Euler(0, 0, -28.8f);

        // move position
        rectTransform.anchoredPosition = new Vector3(-363, -335, 64);

        // remove blur
        blur.SetActive(true);
    }

    public void BringForward()
    {
        // rotate
        rectTransform.rotation = Quaternion.Euler(0, 0, 0);

        // move position
        rectTransform.anchoredPosition = new Vector3(-412.23f, -361.2999f, 44.85482f);

        // remove blur
        blur.SetActive(false);
        rectTransform.SetAsLastSibling();

        id.PushBack();
    }
}
