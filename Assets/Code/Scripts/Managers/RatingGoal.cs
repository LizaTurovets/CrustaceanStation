using UnityEngine;
using UnityEngine.UI;

public class RatingGoal : MonoBehaviour
{
    /*
    TODO: 
        show rating in kiosk
        highlight background green when valid rating
        function to give player extra coins if they beat the goal
    */

    private bool isGoalActive;
    private float rating;
    private float goal;
    [SerializeField] private Slider ratingsSlider;
    [SerializeField] private GameObject ratingsSliderObject;

    [SerializeField] private Image background;

    [SerializeField] private Color valid;
    [SerializeField] private Color notValid;

    [SerializeField] private Slider goalScreenSlider;

    private void Awake()
    {
        ratingsSlider.value = 1;
        isGoalActive = false;
        ratingsSliderObject.SetActive(false);
    }

    public void UpdateRating(float newRating)
    {
        ratingsSlider.value = newRating;
        rating = newRating;

        if (rating >= goal)
        {
            background.color = valid;
        }
        else
        {
            background.color = notValid;
        }
    }

    public float GetRating()
    {
        return rating;
    }

    public bool IsGoalActive()
    {
        return isGoalActive;
    }

    public void SetGoalActive()
    {
        isGoalActive = true;
        ratingsSliderObject.SetActive(true);

        goal = Random.Range(5, 11);
        goalScreenSlider.value = goal / 10f;
    }

    public bool WasGoalAchieved()
    {
        return rating >= goal;
    }
}
