using UnityEngine;
using VisualBehaviorTree.Core;

namespace VisualBehaviorTree.Nodes
{
    internal class SequencerNode : CompositeNode
    {
        public int current;
        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (current >= children.Count)
            {
                Debug.LogError($"Some thing wrong, current child index ({current}) is larger then the expected max index ({children.Count - 1})");
                return State.Failure;
            }

            TreeNode child = children[current];
            switch (child.Update())
            {
                case State.Failure:
                    return State.Failure;
                case State.Running:
                    return State.Running;
                case State.Success:
                    current++;
                    break;
            }

            return current == children.Count ? State.Success : State.Running;
        }
    }
}
