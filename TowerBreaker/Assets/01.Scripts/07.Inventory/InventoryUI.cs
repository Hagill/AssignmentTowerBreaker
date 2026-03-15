using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject itemButtonPrefab;

    private List<float> items;

    public void LoadInventory(List<float> itemList)
    {
        items = itemList;

        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            float currentItem = item;

            GameObject buttonObj = Instantiate(itemButtonPrefab, contentParent);
            Button btn = buttonObj.GetComponent<Button>();

            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"공격력 : {currentItem}";

            btn.onClick.AddListener(() => OnItemButtonClicked(currentItem));
        }
    }

    private void OnItemButtonClicked(float item)
    {
        InventoryManager.Instance.EquipItem(item);
    }
}
