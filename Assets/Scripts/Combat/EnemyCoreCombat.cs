using DigitalMedia.Core;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;

namespace DigitalMedia.Combat
{
    public class EnemyCoreCombat : CoreCombatSystem
    {
        #region Behavior Tree Related Functionality. 
        
        [SerializeField] private bool isAnAgent;
        private NavMeshAgent agent;
        public BehaviourTreeInstance behaviourTreeInstance;
        [SerializeField] private Transform player;

        private BlackboardKey<Vector2> keyRef;
        private BlackboardKey<GameObject> playerKeyRef;
        
        #endregion

        private GameObject airSlash;
        
        void Start()
        {
            _animator = GetComponent<Animator>();
            _audioPlayer = GetComponent<AudioSource>();
            stats = GetComponent<StatsComponent>();
            
            
            playerKeyRef = behaviourTreeInstance.FindBlackboardKey<GameObject>("Player GameObject");
            if (isAnAgent)
            {
                agent = GetComponent<NavMeshAgent>();
                agent.updateRotation = false;
                agent.updateUpAxis = false;
                keyRef = behaviourTreeInstance.FindBlackboardKey<Vector2>("Player Position");
                playerKeyRef.value = player.gameObject;
            }
        }

        private void FixedUpdate()
        {
            if(isAnAgent) keyRef.value = player.transform.position;
        }

        public void SpawnAirSlash()
        {
            
        }
        
    }
}

