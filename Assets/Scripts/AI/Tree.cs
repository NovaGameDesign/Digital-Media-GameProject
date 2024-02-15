using UnityEngine;

namespace DigitalMedia.AI
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        protected EnemyBase S_EnemyBase;
        public static bool runTree = true;

        protected void Start()
        {
            S_EnemyBase = gameObject.GetComponent<EnemyBase>();
            _root = SetupTree();           
        }

        private void Update()
        {
            if(runTree)
            {
                if (_root != null)
                {
                    _root.Evaluate();
                }
            }
           
        }

        public static void setRunTree()
        {
            if(runTree)
            {
                runTree = false;
            }
            else
                runTree = true;
        }

        protected abstract Node SetupTree();
    }

}
