using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    [CreateAssetMenu(fileName = "New Heal Object", menuName = "Inventory System/Items/Heal")]
    public class HealObject : ItemObject
    {
        public int restoreHealthValue;
        public void Reset()
        {
            type = ItemType.Heal;
        }
    }
}
