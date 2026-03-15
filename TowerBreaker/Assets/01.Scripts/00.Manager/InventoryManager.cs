using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonManager<InventoryManager>
{
    private List<float> itemList = new List<float>();
    private float equippedItemAttackPoint = 0f;

    public void AddItem(float itemAttackPoint)
    {
        itemList.Add(itemAttackPoint);
    }

    public List<float> GetItems()
    {
        return itemList;
    }

    public void EquipItem(float itemAttackPoint)
    {
        equippedItemAttackPoint = itemAttackPoint;
    }

    public float GetEquippedItem()
    {
        return equippedItemAttackPoint;
    }
}
