using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace VisualBehaviorTree.BTEditor
{
    public class SplitView : TwoPaneSplitView
    {
        public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }

        public SplitView()
        {

        }
    }
}