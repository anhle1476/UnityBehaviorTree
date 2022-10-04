using UnityEngine;
using VisualBehaviorTree.Core;

namespace VisualBehaviorTree.Nodes
{
    public class DebugLogNode : ActionNode
    {
        public string message = "Debug Node";

        protected override void OnStart()
        {
            //Debug.Log($"OnStart {message}");
        }

        protected override void OnStop()
        {
            //Debug.Log($"OnStop {message}");
        }

        protected override State OnUpdate()
        {
            Debug.Log(message);
            return State.Success;
        }
    }
}