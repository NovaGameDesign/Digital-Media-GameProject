using DigitalMedia.Core;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.U2D.Animation;

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
            spriteLibrary = GetComponent<SpriteLibrary>();
            
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

        public void ForceElementChange()
        {
            currentElement = Elements.Holy;
            currentElementIndex++;
            spriteLibrary.spriteLibraryAsset = elementSprites[currentElementIndex];
          
        }
        
    }
}

