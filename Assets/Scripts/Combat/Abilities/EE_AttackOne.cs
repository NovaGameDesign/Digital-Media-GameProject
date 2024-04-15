using System.Collections;
using System.Collections.Generic;
using DigitalMedia.Combat;
using DigitalMedia.Combat.Abilities;
using UnityEngine;

namespace DigitalMedia
{
    [CreateAssetMenu(menuName = "Abilities/EvilEye/AttackOne", fileName = "EvilEye_AttackOne")]
    public class EE_AttackOne : AbilityBase
    {
       
        [Header("Beam Info")]
        public GameObject beam;

        public float beamDamage;
        
        public override void Activate(GameObject holder)
        {
            var thePosition = holder.transform.TransformPoint(Vector3.right);
            var obj = Instantiate(beam, thePosition, holder.transform.rotation);
        }
    }
}
