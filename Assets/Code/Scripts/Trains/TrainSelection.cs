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

    public Cart.Type GetCartType()
    {
        return cartInfo.cartType;
    } 

    public bool isItFull()
    {
        return isFull;
    }

	private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	public void OnPointerClick(PointerEventData eventData)
    {
        if (isClickable && !isFull)
        {
            // ticket info was wrong
            Cart.Type ticketCartType = kiosk.GetCurrentCrabTicket();
            if (ticketCartType != cartInfo.cartType && kiosk.IsCrabValid()) // if the crab is otherwise valid, but chose the wrong cart
            {
                if (ticketCartType == Cart.Type.Economy)
                {
                    // chosen cart was an upgrade, no rating complaints
                    kiosk.UpgradedCart();
                }
                else if (ticketCartType == Cart.Type.Standard && cartInfo.cartType == Cart.Type.Deluxe)
                {
                    // chosen cart was an upgrade, no rating complaints
                    kiosk.UpgradedCart();
                }
                else
                {
                    kiosk.DowngradedCart(); // chosen cart was a downgrade, rating goes down
                }
            }

            // disappear crab
            kiosk.DisappearCrab();

            // add to how many coins the train has collected 
            trainController.AddToCrabsOnTrain(cartInfo.ticketCost);

            // show that cart is full
            spriteRenderer.sprite = filled;

            isClickable = false;
            isFull = true;
        }
    }


}
