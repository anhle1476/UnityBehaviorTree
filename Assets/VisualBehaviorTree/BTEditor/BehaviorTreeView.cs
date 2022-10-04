using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using VisualBehaviorTree.Core;
using Edge = UnityEditor.Experimental.GraphView.Edge;

namespace VisualBehaviorTree.BTEditor
{
    public class BehaviorTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviorTreeView, GraphView.UxmlTraits> { }

        public Action<NodeView> OnNodeSelected;
        BehaviorTree tree;

        public BehaviorTreeView()
        {
            Insert(0, new GridBackground());

            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/VisualBehaviorTree/BTEditor/BehaviorTreeEditor.uss");
            styleSheets.Add(styleSheet);

            Undo.undoRedoPerformed += OnUndoRedo;
        }

        private void OnUndoRedo()
        {
            PopulateView(tree);
            AssetDatabase.SaveAssets();
        }

        internal void PopulateView(BehaviorTree tree)
        {
            this.tree = tree;

            graphViewChanged -= OnGraphViewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (tree.rootNode == null)
            {
                tree.rootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            // create nodes
            tree.nodes.ForEach(node => CreateNodeView(node));
            // create edges
            tree.nodes.ForEach(node =>
            {
                var children = tree.GetChildren(node);
                children.ForEach(child => CreateEdge(node, child));
            });
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange changes)
        {
            if (changes.elementsToRemove != null)
            {
                changes.elementsToRemove.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (nodeView != null)
                    {
                        tree.DeleteNode(nodeView.node);
                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;

                        tree.RemoveChild(parentView.node, childView.node);
                    }
                });
            }

            if (changes.edgesToCreate != null)
            {
                changes.edgesToCreate.ForEach(edge =>
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;

                    tree.AddChild(parentView.node, childView.node);
                });
            }

            if (changes.movedElements != null)
            {
                changes.movedElements.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    NodeView parent = GetParentNodeView(nodeView);
                    if (parent != null)
                    {
                        parent.SortChildren();
                    }
                });
            }

            return changes;
        }

        public NodeView GetParentNodeView(NodeView nodeView)
        {
            if (nodeView == null || nodeView.input == null || nodeView.input.connected == false) return null;
            Edge inputEdge = nodeView.input.connections.First();
            return inputEdge.output.node as NodeView;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            TypeCache.TypeCollection actionTypes = TypeCache.GetTypesDerivedFrom<ActionNode>();
            TypeCache.TypeCollection decoratorTypes = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            TypeCache.TypeCollection compositeTypes = TypeCache.GetTypesDerivedFrom<CompositeNode>();

            BuildContextMenuForTypes(evt, actionTypes);
            BuildContextMenuForTypes(evt, decoratorTypes);
            BuildContextMenuForTypes(evt, compositeTypes);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        private void BuildContextMenuForTypes(ContextualMenuPopulateEvent evt, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}",
                    action: (a) => CreateNode(type),
                    actionStatusCallback: GetActionStatus);
            }
        }

        private void CreateNode(Type type)
        {
            TreeNode node = tree.CreateNode(type);
            CreateNodeView(node);
        }

        private DropdownMenuAction.Status GetActionStatus(DropdownMenuAction action)
        {
            return !Application.isPlaying && tree != null
                ? DropdownMenuAction.Status.Normal
                : DropdownMenuAction.Status.Disabled;
        }

        private void CreateNodeView(TreeNode node)
        {
            var nodeView = new NodeView(node);
            nodeView.OnNodeSelected = OnNodeSelected;
            AddElement(nodeView);
        }

        private void CreateEdge(TreeNode parent, TreeNode child)
        {
            NodeView parentView = FindNodeView(parent);
            NodeView childView = FindNodeView(child);

            Edge edge = parentView.output.ConnectTo(childView.input);
            AddElement(edge);
        }

        private NodeView FindNodeView(TreeNode node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        public void UpdateNodeStates()
        {
            foreach (var node in nodes)
            {
                var view = node as NodeView;
                view.UpdateState();
            }
        }
    }
}
