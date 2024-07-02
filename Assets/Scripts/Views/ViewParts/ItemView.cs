using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text count;
    
    private ItemData _itemData;

    public void InitItem(ItemData itemData, ItemDescription itemDescription)
    {
        _itemData = itemData;
        icon.sprite = itemDescription.icon;
        gameObject.SetActive(true);

        count.text = itemData.count > 1 ? itemData.count.ToString() : "";
        
    }

    public ItemData GetItemData()
    {
        return _itemData;
    }

    public void SetCount(int newValue)
    {
        
    }

    public void ClearItem()
    {
        _itemData = null;
        gameObject.SetActive(false);
        count.text = "";
    }
}
