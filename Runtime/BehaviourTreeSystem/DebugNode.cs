using UnityEngine;

/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing a debug node in a behaviour tree that logs a message when processed.
    /// </summary>
    public class DebugNode : Node
    {
        private readonly System.Func<string> _messageFunc;
        private readonly Color _gizmoColor;

        public DebugNode(string name,System.Func<string> messageFunc = null,Color? color = null)
        {
            this.name = name;
            _messageFunc = messageFunc;
            _gizmoColor = color ?? Color.yellow;
        }


        /// <summary>
        /// Process the debug node by logging the message returned by the message function.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            if (_messageFunc != null)
                Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(_gizmoColor)}>[DebugNode] {name}: {_messageFunc()} </color>");

            status = Status.SUCCESS;
            return status;
        }
    }

}