using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VisualBehaviorTree.Core
{
    [CreateAssetMenu(fileName = "NewBehaviorTree", menuName = "Behavior Tree", order = 0)]
    public class BehaviorTree : ScriptableObject
    {
        public TreeNode rootNode;
        public TreeNode.State treeState = TreeNode.State.Running;
        public List<TreeNode> nodes = new List<TreeNode>();

        public TreeNode.State Update()
        {
            if (rootNode.state == TreeNode.State.Running)
            {
                treeState = rootNode.Update();
            }

            return treeState;
        }

        public TreeNode CreateNode(Type type)
        {
            var node = ScriptableObject.CreateInstance(type) as TreeNode;
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            nodes.Add(node);

            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(TreeNode node)
        {
            nodes.Remove(node);

            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        public void AddChild(TreeNode parent, TreeNode child)
        {
            parent.AddChild(child);
        }

        public void RemoveChild(TreeNode parent, TreeNode child)
        {
            parent.RemoveChild(child);
        }

        public List<TreeNode> GetChildren(TreeNode parent)
        {
            return parent.GetChildren();
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
            return tree;
        }
    }
}