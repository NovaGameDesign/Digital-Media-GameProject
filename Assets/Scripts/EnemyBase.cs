using DigitalMedia.Core;
using DigitalMedia.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace DigitalMedia
{
   public class EnemyBase : CoreCharacter, IDamageable
   {
      
      public virtual void Attack()
      {
      
      }

      public void WasParried()
      {
         
      }

      private void OnCollisionEnter(Collision other)
      {
         other.gameObject.BroadcastMessage("TouchedAnotherGuy", transform);
      }

      public void DealDamage(float incomingDamage, bool interruptAction = true)
      {
         //write a more complex damage function to account for defense, damage type, etc. 
         /*Debug.Log("The enemy took damage. " +this.gameObject.name);*/
         _health -= incomingDamage;

         if (_health <= 0)
         {
            Destroy(this.gameObject);
         }
      }
   }
}
