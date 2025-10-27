using UnityEngine;
using UnityEngine.EventSystems;

public class DraggingCrab1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private bool dragging = false;
    private RectTransform rectTransform;
    private Vector3 offset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        

    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
