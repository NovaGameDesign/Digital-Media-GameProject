using UnityEngine;
namespace DigitalMedia
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Item/Create New Item")]
    public class Item : ScriptableObject
    {
        //The above menu creates an object to apply the below values onto, along with a sprite for the inventory.
        public int id;
        public string itemName;
        public int value;
        public Sprite icon;
        public ItemType itemType;
        
        public enum ItemType
        {
            //add item types here
            Etank,
            Consumable,
            Throwable
        }
    }
}
