using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    public class ItemPickup : MonoBehaviour
    {
        public Item item;
        void Pickup()
        {
            InventoryManager.Instance.Add(item);
            Destroy(gameObject);
        }
        void OnTriggerEnter()
        {
            Pickup();
        }
    }
}
