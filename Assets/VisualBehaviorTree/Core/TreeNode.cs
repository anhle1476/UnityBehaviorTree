using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using VisualBehaviorTree.Utils;

namespace VisualBehaviorTree.Core
{
    /// <summary>
    /// Represent the graph view port trait for the node
    /// </summary>
    public struct NodePort
    {
        public Orientation Orientation;
        public Direction Direction;
        public Port.Capacity Capacity;
        public Type Type;
    }

    public abstract class TreeNode : ScriptableObject
    {
        public enum State
        {
            Running,
            Failure,
            Success
        }

        [Header("State")]
        [ReadOnly]
        public State state = State.Running;
        [ReadOnly]
        public bool started = false;

        [HideInInspector]
        public string guid;
        [HideInInspector]
        public Vector2 position;

        protected NodePort? _inputPort;
        protected NodePort? _outputPort;

        public TreeNode(NodePort? inputPort, NodePort? outputPort)
        {
            _inputPort = inputPort;
            _outputPort = outputPort;
        }

        public NodePort? InputPort { get => _inputPort; }

        public NodePort? OutputPort { get => _outputPort; }

        public State Update()
        {
            if (!started)
            {
                OnStart();
                started = true;
            }

            state = OnUpdate();

            if (state != State.Running)
            {
                OnStop();
                started = false;
            }

            return state;
        }

        protected abstract void OnStart();
        protected abstract void OnStop();
        protected abstract State OnUpdate();

        internal abstract void AddChild(TreeNode child);
        internal abstract void RemoveChild(TreeNode child);
        internal abstract List<TreeNode> GetChildren();

        /// <summary>
        /// Clone the node so multiple GameObjects running a single tree at the same time won't interfere each other
        /// </summary>
        /// <returns></returns>
        public virtual TreeNode Clone()
        {
            return Instantiate(this);
        }
    }

}
