using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DigitalMedia
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;
        public List<Item> Items = new List<Item>();
        public InventoryItemController[] InventoryItems;
        public Transform ItemContent;
        public GameObject InventoryItem;
        public Toggle EnableRemove;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void Add(Item item)
        {
            Items.Add(item);
        }
        
        public void Remove(Item item)
        {
            Items.Remove(item);
        }
        
        public void ListItems()
        {
            foreach (Transform item in ItemContent)
            {
                Destroy(item.gameObject);
            }
            foreach (var item in Items)
            {
                GameObject obj = Instantiate(InventoryItem, ItemContent);
                var itemName = obj.transform.Find("itemName").GetComponent<Text>();
                var itemIcon = obj.transform.Find("itemIcon").GetComponent<Image>();
                var itemRemove = obj.transform.Find("itemRemove").GetComponent<Button>();
                itemName.text = itemName.itemName;
                itemIcon.sprite = item.icon;
                if (EnableRemove.isOn)
                {
                    itemRemove.gameObject.SetActive(true);
                }
            }
            SetInventoryItems();

        }
        
        public void EnableItemsRemove()
        {
            if (EnableRemove.isOn)
            {
                foreach (Transform item in ItemContent)
                {
                    item.Find("itemRemove").gameObject.SetActive(true);
                }
                
            }
            else
            {
                foreach (Transform item in ItemContent)
                {
                    item.Find("itemRemove").gameObject.SetActive(false);
                }
            }
        }
        public void SetInventoryItems()
        {
            InventoryItems = ItemContent.GetComponentsInChildren<InventoryItemController>();
            for(int i = 0; i < Items.Count; i++)
            {
                InventoryItems[i].AddItem(Items[i]);
            }
        }
    }
}
