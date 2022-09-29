using System;
using System.Collections;
using Unity.VisualScripting.InputSystem;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using VisualBehaviorTree.Core;

namespace VisualBehaviorTree.BTEditor
{
    public class NodeView : Node
    {
        public Action<NodeView> OnNodeSelected;
        public TreeNode node;
        public Port input;
        public Port output;


        public NodeView(TreeNode node) : base("Assets/VisualBehaviorTree/BTEditor/NodeView.uxml")
        {
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            this.style.left = node.position.x;
            this.style.top = node.position.y;

            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();
        }

        private void CreateInputPorts()
        {
            NodePort? portInfo = node.InputPort;

            if (portInfo.HasValue)
            {
                var portVal = portInfo.Value;
                input = InstantiatePort(portVal.Orientation, portVal.Direction, portVal.Capacity, portVal.Type);
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column;
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
                output.style.flexDirection = FlexDirection.ColumnReverse;
                outputContainer.Add(output);
            }
        }

        private void SetupClasses()
        {
            AddToClassList(node.UssClass);
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