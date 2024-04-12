using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace DigitalMedia
{
    public class Rebindscreen : MonoBehaviour
    {
        //Reference Every Rebindable action to disable while rebinding
        public InputActionReference MoveRef, JumpRef, DodgeRef, AttackRef, MenuRef;
        void Start ()
        {
            
        }
        void OnEnable() 
        {
            MoveRef.action.Disable();
            JumpRef.action.Disable();
            DodgeRef.action.Disable();
            AttackRef.action.Disable();
            MenuRef.action.Disable();
        }
        void OnDisable() 
        {
            MoveRef.action.Enable();
            JumpRef.action.Enable();
            DodgeRef.action.Enable();
            AttackRef.action.Enable();
            MenuRef.action.Enable();
        }

       
    }
}
