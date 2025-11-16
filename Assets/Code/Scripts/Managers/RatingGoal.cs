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
        
    }

    public bool WasGoalAchieved()
    {
        return rating >= goal;
    }
}
 