using System;

/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing a condition node in a behaviour tree that evaluates a boolean condition.
    /// </summary>
    public class ConditionNode : Node
    {
        private Func<bool> _condition;

        public ConditionNode(string name, Func<bool> condition)
        {
            this.name = name;
            _condition = condition;
        }

        /// <summary>
        /// Process the condition node by evaluating the condition function.
        /// Returns SUCCESS if the condition is true, otherwise returns FAILURE.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            if (_condition())
            {
                DebugStatus(Status.SUCCESS);
                return Status.SUCCESS;
            }
            else
            {
                DebugStatus(Status.FAILURE);
                return Status.FAILURE;
            }
        }
    }

}