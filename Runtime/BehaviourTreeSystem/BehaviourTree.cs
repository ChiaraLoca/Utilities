using System.Collections.Generic;
using UnityEngine;

/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing the root of a behaviour tree.
    /// </summary>
    public class BehaviourTree : Node
    {
        public BehaviourTree() { 
        
            name = "Root";
        }

        public BehaviourTree(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Process the behaviour tree starting from the root node.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            if (children.Count == 0) {
                status = Status.SUCCESS;
                return Status.SUCCESS;
            }
            status = children[currentChild].Process();
            return status;
        }

        // Helper struct to keep track of node levels during tree printing
        struct NodeLevel
        { 
            public int level;
            public Node node;

            public NodeLevel(int level, Node node)
            {
                this.level = level;
                this.node = node;
            }
        }

        /// <summary>
        /// Print the behaviour tree structure to the console.
        /// </summary>
        public void PrintTree()
        {
            string treePrintout = "";
            Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
            Node currentNode = this;
            nodeStack.Push(new NodeLevel(0,currentNode));

            while (nodeStack.Count != 0)
            {
                NodeLevel nextNode = nodeStack.Pop();
                treePrintout += new string('-',nextNode.level) + nextNode.node.name +"\n";
                for (int i = nextNode.node.children.Count - 1;i>=0; i--)
                {
                    nodeStack.Push(new NodeLevel( nextNode.level+1, nextNode.node.children[i]));
                }
            }

            Debug.Log(treePrintout);
        }
    }
}