using DigitalMedia.Core;
using UnityEngine;

namespace DigitalMedia.Interfaces
{
    public interface IDamageable
    {
        /// <summary>
        /// Method to deal damage to a target. You must pass the desired damage dealt and attack origin. There is a final additional option that let's us denote whether the target's current action should stop. 
        /// </summary>
        /// <param name="incomingDamage"></param>
        /// <param name="attackOrigin"></param>
        /// <param name="knockbackForce"></param>
        /// <param name="interruptAction"></param>
        public void DealDamage(float incomingDamage, GameObject attackOrigin, Elements damageType, float knockbackForce = .5f, bool interruptAction = true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="incomingVitalityDamage"></param>
        public void DealVitalityDamage(float incomingVitalityDamage);

    }
}