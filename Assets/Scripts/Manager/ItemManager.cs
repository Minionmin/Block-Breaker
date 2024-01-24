using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    /// <summary> Droppable item types</summary>
    public enum ItemTypes
    {
        Enlarge,
        Piercing,
        SecondLife
    }

    /// <summary> Current active item list </summary>
    private List<Item> items;

    /// <summary> Droppable item type list </summary>
    public List<Item> itemTypes;

    private void Awake()
    {
        Instance = this;
        items = new List<Item>();
    }

    /// <summary> Update item status/effect every frame if one has </summary>
    private void Update()
    {
        if (items.Count > 0)
        {
            UpdateItemStatus();
        }
    }

    /// <summary> Update item effect </summary>
    private void UpdateItemStatus()
    {
        for(int i = 0; i < items.Count; i++)
        {
            // If item has not been hit by the ball yet, do nothing
            if (!items[i].isActivated) continue;

            items[i].Tick();
        }
    }

    /// <summary> Drop item when the block get destroyed </summary>
    /// <param name="itemType"> Dropping item type </param>
    public void DropItem(ItemTypes itemType, Transform dropPosition)
    {
        var item = Instantiate(itemTypes[(int)itemType], dropPosition.position,Quaternion.identity, transform);
        items.Add(item);
    }

    /// <summary> Remove the item from list to deactivate it </summary>
    public void DeactivateItem(Item item)
    {
        item.isActivated = false;
        items.Remove(item);
        Destroy(item.gameObject);
    }

    /// <summary> Clear all the items from the list when changing level </summary>
    public void ClearItem()
    {
        int count = items.Count;

        while(count > 0)
        {
            DeactivateItem(items[0]);
            count--;
        }
    }

    /// <summary> Get ItemTypes enum size </summary>
    public int GetItemTypeCount() { return System.Enum.GetValues(typeof(ItemTypes)).Length; }
}
