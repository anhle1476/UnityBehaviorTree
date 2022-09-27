﻿using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VisualBehaviorTree.Utils;

namespace VisualBehaviorTree.Core
{
    public abstract class CompositeNode : TreeNode
    {
        [Header("Properties")]
        [ReadOnly]
        public List<TreeNode> children = new List<TreeNode>();

        public CompositeNode() : base(
            inputPort: new NodePort
            {
                Orientation = Orientation.Vertical,
                Direction = Direction.Input,
                Capacity = Port.Capacity.Single,
                Type = typeof(bool)
            },
            outputPort: new NodePort
            {
                Orientation = Orientation.Vertical,
                Direction = Direction.Output,
                Capacity = Port.Capacity.Multi,
                Type = typeof(bool)
            })
        {

        }

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