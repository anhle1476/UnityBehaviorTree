using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace VisualBehaviorTree.Core
{
    public abstract class ActionNode : TreeNode
    {

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