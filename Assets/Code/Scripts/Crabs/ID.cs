using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ID : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    private string crabName;

    [SerializeField] private Image image;
    private Sprite sprite;


    public void SetName(string newName)
    {
        crabName = newName;
        nameText.text = crabName;
    }

    public void SetIDPhoto(Sprite newSprite)
    {
        sprite = newSprite;
        image.sprite = sprite;
    }
}
