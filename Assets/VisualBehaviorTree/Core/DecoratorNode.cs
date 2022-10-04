using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VisualBehaviorTree.Utils;

namespace VisualBehaviorTree.Core
{
    public abstract class DecoratorNode : TreeNode
    {
        [ReadOnly]
        public TreeNode child;

        public override NodePort? OutputPort => new NodePort
        {
            Orientation = Orientation.Vertical,
            Direction = Direction.Output,
            Capacity = Port.Capacity.Single,
            Type = typeof(bool)
        };

        public override string UssClass => nameof(DecoratorNode);

        internal override void AddChild(TreeNode child)
        {
            this.child = child;
        }

        internal override void RemoveChild(TreeNode child)
        {
            if (this.child == null) return;

            if (this.child == child)
            {
                this.child = null;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning($"Try to remove an invalid child ({child.name}) from decorator node ({name}). The currrent child is ({this.child.name})");
            }
#endif
        }

        internal override List<TreeNode> GetChildren()
        {
            var result = new List<TreeNode>();
            if (child != null)
            {
                result.Add(child);
            }

            return result;
        }

        public override TreeNode Clone()
        {
            var node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}