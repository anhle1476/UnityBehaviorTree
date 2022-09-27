using System.Collections.Generic;
using UnityEngine;
using VisualBehaviorTree.Core;
using VisualBehaviorTree.Nodes;

namespace VisualBehaviorTree
{
    public class BehaviorTreeRunner : MonoBehaviour
    {
        public BehaviorTree tree;

        // Start is called before the first frame update
        void Start()
        {
            if (tree != null)
            {
                tree = tree.Clone();
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning($"Runner of {gameObject.name} missing Behavior Tree");
            }
#endif
        }

        // Update is called once per frame
        private void Update()
        {
            if (tree != null && tree.rootNode != null)
            {
                tree.Update();
            }
        }
    }
}
