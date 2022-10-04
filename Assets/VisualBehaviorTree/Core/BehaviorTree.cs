using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VisualBehaviorTree.BTEditor;

namespace VisualBehaviorTree.Core
{
    [CreateAssetMenu(fileName = "NewBehaviorTree", menuName = "Behavior Tree", order = 0)]
    public class BehaviorTree : ScriptableObject
    {
        public TreeNode rootNode;
        public TreeNode.State treeState = TreeNode.State.Running;
        public List<TreeNode> nodes = new();
        public Blackboard blackboard = new();

        public TreeNode.State Update()
        {
            if (rootNode.state == TreeNode.State.Running)
            {
                treeState = rootNode.Update();
            }

            return treeState;
        }

#if UNITY_EDITOR
        public TreeNode CreateNode(Type type)
        {
            var node = ScriptableObject.CreateInstance(type) as TreeNode;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, $"{nameof(BehaviorTree)} ({nameof(CreateNode)})");
            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            Undo.RegisterCreatedObjectUndo(node, $"{nameof(BehaviorTree)} ({nameof(CreateNode)})");
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(TreeNode node)
        {
            Undo.RecordObject(this, $"{nameof(BehaviorTree)} ({nameof(DeleteNode)})");
            nodes.Remove(node);

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(TreeNode parent, TreeNode child)
        {
            Undo.RecordObject(parent, $"{nameof(BehaviorTree)} ({nameof(AddChild)})");
            parent.AddChild(child);
            EditorUtility.SetDirty(parent);
        }

        public void RemoveChild(TreeNode parent, TreeNode child)
        {
            Undo.RecordObject(parent, $"{nameof(BehaviorTree)} ({nameof(RemoveChild)})");
            parent.RemoveChild(child);
            EditorUtility.SetDirty(parent);
        }
#endif

        public List<TreeNode> GetChildren(TreeNode parent)
        {
            return parent.GetChildren();
        }

        protected void Traverse(TreeNode node, Action<TreeNode> visiter)
        {
            if (!node) return;
            var children = node.GetChildren();
            foreach (TreeNode child in children)
            {
                visiter.Invoke(child);
                Traverse(child, visiter);
            }
        }

        public BehaviorTree Clone()
        {
            if (rootNode == null)
            {
                rootNode = CreateNode(typeof(RootNode));
                AssetDatabase.SaveAssets();
            }
            BehaviorTree tree = Instantiate(this);
            tree.rootNode = tree.rootNode.Clone();
            tree.nodes = new List<TreeNode>();

            Traverse(tree.rootNode, (node) => tree.nodes.Add(node));

            return tree;
        }
    }
}