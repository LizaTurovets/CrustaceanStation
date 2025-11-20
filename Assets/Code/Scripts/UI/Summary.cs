using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI crabsProcessed;
    [SerializeField] private Slider ratings;

    public void SetCrabsProcessed(int crabs)
    {
        crabsProcessed.text = crabs.ToString();
    }

    public void SetRating(float rating)
    {
        ratings.value = rating;
    }
}
