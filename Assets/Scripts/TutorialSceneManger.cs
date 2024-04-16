using System.Collections;
using System.Collections.Generic;
using DigitalMedia.Combat;
using UnityEngine;

namespace DigitalMedia
{
    public class TutorialSceneManger : MonoBehaviour
    {
        [SerializeField] private GameObject[] tutorialUI;
        private int tutorialIndex;

        [SerializeField] private PlayerController _playerController;

        [SerializeField] private PlayerCombatSystem _playerCombatSystem;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
