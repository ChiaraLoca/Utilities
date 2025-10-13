/// 
/// @author: Chiara Locatelli
/// 
namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing a leaf node in a behaviour tree that executes a specific action or condition.
    /// </summary>
    public class Loop : Node
    {
        /// <summary>
        /// This behavior tree has to be based on conditions. You don't want any actions.
        /// </summary>
        private BehaviourTree _complexCondition;
        private ConditionNode _simpleCondition;
        
        public Loop(string name, BehaviourTree complexCondition)
        {
            this.name = name;
            _complexCondition = complexCondition;
        }

        public Loop(string name, ConditionNode simpleCondition)
        {
            this.name = name;
            _simpleCondition = simpleCondition;
        }

        /// <summary>
        /// Process the loop node by evaluating its condition and processing its children in order.
        /// Returns SUCCESS if the condition fails, otherwise processes its children in a loop.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            bool conditionFailed = false;
            
            if (_simpleCondition != null)
                conditionFailed = _simpleCondition.Process() == Status.FAILURE;
            else
                conditionFailed = _complexCondition.Process() == Status.FAILURE;

            if (conditionFailed)
            {
                DebugStatus(Status.SUCCESS);
                return Status.SUCCESS;
            }

            Status childStatus = children[currentChild].Process();
            switch (childStatus)
            {
                case Status.RUNNING:
                    DebugStatus(Status.RUNNING);
                    return Status.RUNNING;

                case Status.FAILURE:
                    foreach (Node n in children)
                        n.Reset();
                    currentChild = 0;
                    DebugStatus(Status.FAILURE);
                    return Status.FAILURE;

                case Status.SUCCESS:
                    currentChild++;
                    if (currentChild >= children.Count)
                    {
                        currentChild = 0;
                        
                        foreach (Node n in children)
                            n.Reset();
                        DebugStatus(Status.RUNNING);
                    }
                    return Status.RUNNING;
            }

            DebugStatus(Status.RUNNING);
            return Status.RUNNING;

        }
    }

}