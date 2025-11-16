using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
public class TrainSelection : MonoBehaviour, IPointerClickHandler
{
    private bool isClickable = false;
    private bool isFull = false;
    private Kiosk kiosk;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite filled;
    private TrainController trainController;

    // CART INFO
    [SerializeField] private Cart cartInfo;

    public void SetThisClickable(bool newIsClickable)
    {
        isClickable = newIsClickable;
    }
 
    public void SetKiosk(Kiosk newKiosk)
    {
        kiosk = newKiosk;
    }

    public void SetController(TrainController newController)
    {
        trainController = newController;
    }

    public float getStartingPos()
    {
        return cartInfo.cartStartingPoint;
    }

    public float getHeight()
    {
        return cartInfo.cartHeight;
    }

	private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLICK");
        if (isClickable && !isFull)
        {
            // disappear crab
            kiosk.DisappearCrab();

            // add to capacity 
            trainController.AddToCrabsOnTrain(cartInfo.ticketCost);

            // show that cart is full
            spriteRenderer.sprite = filled;

            isClickable = false;
            isFull = true;
        }
    }


}
