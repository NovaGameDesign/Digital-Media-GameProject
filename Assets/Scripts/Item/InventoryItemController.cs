using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace DigitalMedia
{
    public class InventoryItemController : MonoBehaviour
    {
        Item item;
        public Button itemRemove;
        public void RemoveItem()
        {
            InventoryManager.Instance.Remove(item);
            Destroy(gameObject);
        }
        public void AddItem(Item newItem)
        {
            item= newItem;

        }
        public void UseItem()
        {
            switch (item.itemType)
            {
                //case format example, add similar methods to the player
                /*
                 * case Item.Itemtype.Etank:
                 * increase health method gets called here
                break;
                

                */
            }
        }
    }
}
