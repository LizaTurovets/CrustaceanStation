using UnityEngine;

public class Shopkeeper : MonoBehaviour
{
    [SerializeField] private GameObject wares;
    [SerializeField] private RectTransform rectTransform;
    private bool isMoving = false;
    private Vector3 endPos = new Vector3(470.9427f, -31.97021f, 0);
    private Vector3 currentVelocity;
    private bool presented = false;

    private void Awake()
    {
        isMoving = true;
        presented = false;
        wares.SetActive(false);
    }

    private void Update()
    {
        if (isMoving)
        {

            rectTransform.anchoredPosition = Vector3.SmoothDamp(rectTransform.anchoredPosition, endPos, ref currentVelocity, 0.25f);

            if (Vector2.Distance(rectTransform.anchoredPosition, endPos) < 10f && !presented)
            {
                presented = true;
                PresentWares();
            }
            else if (Vector2.Distance(rectTransform.anchoredPosition, endPos) < 0.1f)
            {
                isMoving = false;
                rectTransform.anchoredPosition = endPos;

            }


        }
    }

    private void PresentWares()
    {
        //show all buttons
        wares.SetActive(true);
    }
}
