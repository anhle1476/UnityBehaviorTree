using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VisualBehaviorTree.Core;

namespace VisualBehaviorTree.BTEditor
{
    public class NodeView : Node
    {
        public Action<NodeView> OnNodeSelected;
        public TreeNode node;
        public Port input;
        public Port output;


        public NodeView(TreeNode node)
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            this.style.left = node.position.x;
            this.style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
        }

        private void CreateInputPorts()
        {
            NodePort? portInfo = node.InputPort;

            if (portInfo.HasValue)
            {
                var portVal = portInfo.Value;
                input = InstantiatePort(portVal.Orientation, portVal.Direction, portVal.Capacity, portVal.Type);
                input.portName = "";
                inputContainer.Add(input);
            }
        }

        private void CreateOutputPorts()
        {
            NodePort? portInfo = node.OutputPort;

            if (portInfo.HasValue)
            {
                var portVal = portInfo.Value;
                output = InstantiatePort(portVal.Orientation, portVal.Direction, portVal.Capacity, portVal.Type);
                output.portName = "";
                outputContainer.Add(output);
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            node.position.x = newPos.xMin;
            node.position.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }
    }
}