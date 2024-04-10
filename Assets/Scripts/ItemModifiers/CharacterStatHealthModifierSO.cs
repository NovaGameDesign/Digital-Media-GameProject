using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalMedia
{
    [CreateAssetMenu]
    public class CharacterStatHealthModifierSO : CharacterStatModifierSO
    {
        public override void AffectCharacter(GameObject character, float val)
        {
            PlayerStats health= character.GetComponent<PlayerStats>();
            if (health != null)
                health.AddHealth((int)val);
        }
    }
}
