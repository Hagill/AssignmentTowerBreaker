using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonManager<InventoryManager>
{
    private List<Item> itemList = new List<Item>();
    private Item equippedItem;

    public void AddItem(Item item)
    {
        itemList.Add(item);
    }

    public List<Item> GetItems()
    {
        return itemList;
    }

    public void EquipItem(Item item)
    {
        equippedItem = item;
    }
    public Item GetEquippedItem()
    {
        return equippedItem;
    }
}
