using TMPro;
using UnityEngine;

public class CrabCountGoal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goalText;
    private int goalCount;

    private void Awake()
    {
        goalText.text = "";
    }

    public void SetGoalActive()
    {
        goalCount = Random.Range(20, 40);
        goalText.text = goalCount.ToString();
    }
}
