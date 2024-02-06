using UnityEngine;
using UnityEngine.InputSystem;

namespace DigitalMedia.Combat
{
    public class CombatSystem : MonoBehaviour
    {
        //Input Related
        private PlayerInput _playerInput;
        private InputAction attack;
    
        private int currentAttackIndex;
        private Animator _animator;
    
        private const string PLAYER_IDLE = "Player_Idle";
        private const string PLAYER_ATTACK_ONE = "Player_Attack1";

        [SerializeField] private CharacterStats data; 
    

        private void Start()
        {
            //Input
            _playerInput = GetComponent<PlayerInput>();
            attack = _playerInput.actions["Attack"];
            attack.performed += TryAttack;
        
            _animator = GetComponent<Animator>();
        }

        private void TryAttack(InputAction.CallbackContext context)
        {
            Debug.Log("Attacked");
            _animator.Play(PLAYER_ATTACK_ONE);
        }

        public void EndAttackSequence()
        {
            _animator.Play(PLAYER_IDLE);
        }

        public void TryToParry()
        {
        
        }

        private void OnDrawGizmosSelected()
        {
            if(data == null)
                return;
            Gizmos.DrawWireCube(transform.position + (Vector3)data.CombatData.parryOffset, (Vector3)data.CombatData.parryRange);
        }
    }
}
