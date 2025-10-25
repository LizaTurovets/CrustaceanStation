using UnityEngine;

public class CrabController : MonoBehaviour
{
    [SerializeField] private CrabInfo crabInfo;
    private RectTransform rectTransform;

    [SerializeField] private Canvas canvas;

    
    // TICKET AND ID
    [SerializeField] private GameObject ticketPrefab;
    private GameObject ticket;
    [SerializeField] private GameObject idPrefab;
    private GameObject id;
    private CrabSelector crabSelector;
    private string trainID = "";


    // VALIDITY
    private bool isValid = true;


    //MOVEMENT
    private bool isMoving = false;
    [SerializeField] private float speed = 50f;
    private bool approachingKiosk = false;
    private bool leavingKiosk = false;
    private Vector3 kioskEndPos; // where we want the crab to be when it's at the kiosk
    private Vector3 kioskStartPos; // where we want the crab to pop out from when it approaches the kiosk


    //MISC
    private Clock clock;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        kioskStartPos = new Vector3(-470, -500, 0);
        kioskEndPos = new Vector3(-470, 140, 0);
        rectTransform.anchoredPosition = kioskStartPos;
	}

    public void SetCanvas(Canvas newCanvas)
    {
        canvas = newCanvas;
    }
    public void SetCrabSelector(CrabSelector newSelector)
    {
        crabSelector = newSelector;
    }
    
    public void SetClock(Clock newClock) {
        clock = newClock;
    }
    public void PresentTicketAndID()
    {
        ticket = Instantiate(ticketPrefab, canvas.transform);
        id = Instantiate(idPrefab, canvas.transform);


        // SOMETIMES GENERATE MISMATCHING INFO
        string crabName = crabInfo.crabName;
        Sprite crabPhoto = crabInfo.sprite;
        if (Random.Range(0, 10) > 8)                    // RANDOM NAME
        {
            crabName = crabSelector.ChooseName();
            if (crabName != crabInfo.crabName)
            {
                isValid = false;
            }
        }
        if (Random.Range(0, 10) > 8)               // RANDOM SPRITE
        {
            crabPhoto = crabSelector.ChooseSprite();
            if (crabPhoto != crabInfo.sprite)
            {
                isValid = false;
            }
        }

        // FIGURE OUT WHICH DOCUMENT IS FORGED
        if (!isValid)
        {
            if (Random.Range(0, 10) > 8)                                       // FORGED ID
            {
                ticket.GetComponent<Ticket>().SetName(crabName);
                id.GetComponent<ID>().SetName(crabInfo.crabName);
            }
            else                                                               // FORGED TICKET
            {
                ticket.GetComponent<Ticket>().SetName(crabInfo.crabName);
                id.GetComponent<ID>().SetName(crabName);
            }
        }
        else // if not forged
        {
            ticket.GetComponent<Ticket>().SetName(crabName);
            id.GetComponent<ID>().SetName(crabName);
        }

        id.GetComponent<ID>().SetIDPhoto(crabPhoto);

        if (Random.Range(0, 10) > 8)                                            // MORE FORGERY! - TRAIN ID
        {
            trainID = ticket.GetComponent<Ticket>().GetRandomTrainID();
        }
        else
        {
            trainID = clock.GetRandomCurrentTrainID();
            
        }
        ticket.GetComponent<Ticket>().SetTrainID(trainID);

    }

    public string GetTrainID()
    {
        return trainID;
    }



    public bool IsValid()
    {
        return isValid;
    }
    public void RemoveTicketAndID()
    {
        Destroy(ticket);
        Destroy(id);
    }

    [ContextMenu("MakeAppear")]
    public void MakeAppear()
    {
        isMoving = true;
        approachingKiosk = true;
        leavingKiosk = false;
    }
    [ContextMenu("MakeDisappear")]
    public void MakeDisappear()
    {
        isMoving = true;
        approachingKiosk = false;
        leavingKiosk = true;
    }

	private void Update()
    {
        if (isMoving)
        {
            if (approachingKiosk)
            {
                var step = speed * Time.deltaTime;
                rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, kioskEndPos, step);

                if (rectTransform.anchoredPosition.y >= kioskEndPos.y)
                {
                    approachingKiosk = false;
                    isMoving = false;
                    rectTransform.anchoredPosition = kioskEndPos;
                    PresentTicketAndID();
                }
                
            }
            else if (leavingKiosk)
            {
                var step = speed * Time.deltaTime;
                rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, kioskStartPos, step);

                if (rectTransform.anchoredPosition.y <= kioskStartPos.y)
                {
                    leavingKiosk = false;
                    isMoving = false;
                    rectTransform.anchoredPosition = kioskStartPos;
                    RemoveTicketAndID();
                }
                
            }


        }
    }


}
