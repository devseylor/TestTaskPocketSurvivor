using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour
{
    [SerializeField] private ItemView item;  
    [SerializeField] private RectTransform thisRect;

    public void InitItem(ItemData itemData, ItemDescription itemDescription, PointerEventData eventData)
    {
        item.InitItem(itemData, itemDescription);
        eventData.pointerEnter = gameObject;
        MoveItem(eventData);
    }
    
    public void MoveItem(PointerEventData eventData)
    {
        thisRect.position = eventData.position;
    }
}