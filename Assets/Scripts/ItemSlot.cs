using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler, IEndDragHandler, IPointerClickHandler, IDragHandler
{
    [SerializeField] private ItemView item;

    private int _index;
    
    public Action OnClickSlot;
    public Action<PointerEventData, int> OnDragItem;
    public Action<int> OnDropItem;
    public Action OnEndDragItem;
    public void InitItem(int index, ItemData itemData, ItemDescription itemDescription)
    {
        _index = index;
        item.InitItem(itemData, itemDescription);
    }
    
    public void SetIndex(int index)
    {
        _index = index;
    }
    
    public void ClearItem()
    {
        item.ClearItem();
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragItem?.Invoke(eventData, _index);
    }
    
    public void OnDrop(PointerEventData eventData)
    {
            OnDropItem?.Invoke(_index);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        OnClickSlot?.Invoke();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragItem?.Invoke();
    }
}