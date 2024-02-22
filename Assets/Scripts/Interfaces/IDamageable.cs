using UnityEngine;

namespace DigitalMedia.Interfaces
{
    public interface IDamageable
    {
        public void DealDamage(float incomingDamage, GameObject attackOrigin, bool interruptAction = true);

        public void DealVitalityDamage(float incomingVitalityDamage);

    }
}