using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VisualBehaviorTree.Utils;

namespace VisualBehaviorTree.Core
{
    public abstract class CompositeNode : TreeNode
    {
        [ReadOnly]
        public List<TreeNode> children = new();

        public override NodePort? OutputPort => new NodePort
        {
            Orientation = Orientation.Vertical,
            Direction = Direction.Output,
            Capacity = Port.Capacity.Multi,
            Type = typeof(bool)
        };

        public override string UssClass => nameof(CompositeNode);

        internal override void AddChild(TreeNode child)
        {
            children.Add(child);
        }

        internal override void RemoveChild(TreeNode child)
        {

            children.Remove(child);
        }

        internal override List<TreeNode> GetChildren()
        {
            return children;
        }

        public override TreeNode Clone()
        {
            var node = Instantiate(this);
            node.children = children.ConvertAll(child => child.Clone());
            return node;
        }
    }
}