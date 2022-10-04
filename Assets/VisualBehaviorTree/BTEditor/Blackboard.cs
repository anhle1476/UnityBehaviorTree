using System;
using System.Collections;
using UnityEngine;

namespace VisualBehaviorTree.BTEditor
{
    [Serializable]
    public class Blackboard
    {
        public Vector3 moveToPosition;
        public GameObject moveToObject;
    }
}