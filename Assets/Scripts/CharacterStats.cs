using System;
using UnityEngine;

namespace DigitalMedia
{
    [Serializable]
    public class StatsRelated
    {
        [Header("Main Stats")]
        public float health;
        /// <summary>
        /// Defense is a precent reduction to incoming damage
        /// </summary>
        public float defense;
        /// <summary>
        /// Stamina is an internal stat we probably won't display to the player. We will use this on this enemy to keep track of their attacks, as well as to prevent the player from spamming.
        /// </summary>
        public float stamina;
        
        [Header("Speed")] 
        public float speed;
        /// <summary>
        /// Speed modifier is the amount speed is increased if/when a buff is applied. For example critical hits may give the player a slight speed buff. 
        /// </summary>
        public float speedModifier;
    }

    [Serializable]
    public class WeaponInfo
    {
        public float innateWeaponDamage;
        public float innateWeaponBlock;
        public float innateWeaponStaminaDrain;
        public int hitComboLength;
    
    }

    [Serializable]
    public class CombatRelated
    {
        [Header("Damage Related")]
        public float attackPower;

        public WeaponInfo weaponData;

        public Vector2 parryRange;
        public Vector2 parryOffset;

    }

    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharacterStats", order = 1)]
    public class CharacterStats : ScriptableObject
    {
        public StatsRelated BasicData;
        public CombatRelated CombatData;

    }
}