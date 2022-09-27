﻿using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VisualBehaviorTree.Utils;

namespace VisualBehaviorTree.Core
{
    public class RootNode : TreeNode
    {
        [Header("Properties")]
        [ReadOnly]
        public TreeNode child;

        public RootNode() : base(
            null,
            outputPort: new NodePort
            {
                Orientation = Orientation.Vertical,
                Direction = Direction.Output,
                Capacity = Port.Capacity.Single,
                Type = typeof(bool)
            })
        {

        }

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

        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            child?.Update();
            return state; // always running
        }

        public override TreeNode Clone()
        {
            var node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}