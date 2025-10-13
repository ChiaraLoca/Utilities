using System.Collections.Generic;
using UnityEngine;

/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Base class representing a node in a behaviour tree.
    /// </summary>
    public class Node
    {
        public Status status;
        public List<Node> children = new List<Node>();
        public int currentChild = 0;
        public string name;
        public int priority = 0;
        
        /// <summary>
        /// Enable or disable debug logging for all nodes.
        /// </summary>
        public static bool DebugEnabled = true;

        public Node(string name)
        {
            this.name = name;
        }

        public Node(string name, int priority)
        {
            this.name = name;
            this.priority = priority;
        }

        public Node() { }

        /// <summary>
        /// Process the node by executing its associated behavior.
        /// </summary>
        /// <returns></returns>
        public virtual Status Process()
        {
            return children[currentChild].Process();
        }

        /// <summary>
        /// Add a child node to this node.
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(Node child)
        {
            children.Add(child);
        }

        /// <summary>
        /// Reset the node and its children to their initial state.
        /// </summary>
        internal void Reset()
        {
            foreach (Node child in children)
            {
                child.Reset();
            }
            currentChild = 0;
        }

        /// <summary>
        /// Debug the status of the node if it has changed.
        /// </summary>
        /// <param name="newStatus"></param>
        protected void DebugStatus(Status newStatus)
        {
            if (!DebugEnabled) return;

            if (newStatus != status)
            {
                Color color = Color.white;
                switch(newStatus)
                {
                    case Status.SUCCESS:
                        color = Color.green;
                        break;
                    case Status.FAILURE:
                        color = Color.red;
                        break;
                    case Status.RUNNING:
                        color = Color.yellow;
                        break;
                }

                Debug.Log($"[{GetType().Name}] {name} -> <color=#{ColorUtility.ToHtmlStringRGBA(color)}>{newStatus}</color>");
                status = newStatus;
            }
        }
        /// <summary>
        /// Remove a child node from this node.
        /// </summary>
        /// <param name="child"></param>
        public void RemoveChild(Node child)
        {
            children.Remove(child);
        }

        /// <summary>
        /// Remove all child nodes from this node.
        /// </summary>
        public void RemoveChildren()
        {
            children.Clear();
        }   

        /// <summary>
        /// Enumeration representing the possible statuses of a node.
        /// </summary>
        public enum Status
        {
            SUCCESS,
            RUNNING,
            FAILURE
        }
    }
}