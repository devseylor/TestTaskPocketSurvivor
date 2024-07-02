using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private Image image;

    private Vector2 _deltaPos;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        _deltaPos = eventData.position - thisRect.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        image.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        thisRect.anchoredPosition += eventData.delta;
    }
}