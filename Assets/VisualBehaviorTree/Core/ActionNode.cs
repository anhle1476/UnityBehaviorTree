using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace VisualBehaviorTree.Core
{
    public abstract class ActionNode : TreeNode
    {

        public ActionNode() : base(
            inputPort: new NodePort
            {
                Orientation = Orientation.Vertical,
                Direction = Direction.Input,
                Capacity = Port.Capacity.Single,
                Type = typeof(bool)
            },
            outputPort: null)
        {

        }

        internal override void AddChild(TreeNode child)
        {
            // no child -> no action
        }

        internal override void RemoveChild(TreeNode child)
        {
            // no child -> no action
        }

        internal override List<TreeNode> GetChildren()
        {
            return new List<TreeNode>(); ;
        }
    }
}