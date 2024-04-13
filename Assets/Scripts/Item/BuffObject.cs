using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    [CreateAssetMenu(fileName = "New Buff Object", menuName = "Inventory System/Items/Buff")]
    public class BuffObject : ItemObject
    {
        public float atkBonus;
        public float defenseBonus;
        public void Reset()
        {
            type = ItemType.Buff;
        }
    }
}
