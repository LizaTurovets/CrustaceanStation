using UnityEngine;
using UnityEngine.EventSystems;
public class TrainSelection : MonoBehaviour, IPointerClickHandler
{
    private bool isClickable = false;
    private Kiosk kiosk;
    [SerializeField] private TrainController trainController;

    public void SetThisClickable(bool newIsClickable)
    {
        isClickable = newIsClickable;
    }

    public void SetKiosk(Kiosk newKiosk)
    {
        kiosk = newKiosk;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClickable)
        {
            // disappear crab
            kiosk.DisappearCrab();

            // add to capacity 
            trainController.AddToCrabsOnTrain();


            isClickable = false;
        }
    }


}
