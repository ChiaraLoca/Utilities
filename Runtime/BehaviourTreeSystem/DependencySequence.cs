/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing a dependency sequence node in a behaviour tree that processes its children
    /// </summary>
    public class DependencySequence : Node
    {
        /// <summary>
        /// This behavior tree has to be based on conditions. You don't want any actions.
        /// </summary>
        private BehaviourTree _complexCondition;
        private ConditionNode _simpleCondition;
        
        public delegate void DependencyOnFail();
        private DependencyOnFail DependencyOnFailMethod;

        public DependencySequence(string name,BehaviourTree complexCondition, DependencyOnFail dependencyOnFailMethod =null)
        {
            this.name = name;
            _complexCondition = complexCondition;
            DependencyOnFailMethod = dependencyOnFailMethod;
        }

        public DependencySequence(string name, ConditionNode simpleCondition, DependencyOnFail dependencyOnFailMethod = null)
        {
            this.name = name;
            _simpleCondition = simpleCondition;
            DependencyOnFailMethod = dependencyOnFailMethod;
        }

        /// <summary>
        /// Process the dependency sequence node by evaluating its condition and processing its children in order.
        /// Returns FAILURE if the condition fails or any child returns FAILURE.
        /// returns SUCCESS if all children succeed.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            bool conditionFailed = false;
            if (_simpleCondition != null)

                conditionFailed = _simpleCondition.Process() == Status.FAILURE;
            else
                conditionFailed = _complexCondition.Process() == Status.FAILURE;
            if(conditionFailed)
            {
                if (DependencyOnFailMethod != null)
                    DependencyOnFailMethod();
               
                foreach (Node child in children)
                    child.Reset();

                DebugStatus(Status.FAILURE);
                return Status.FAILURE;
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
                        DebugStatus(Status.SUCCESS);
                        return Status.SUCCESS;
                    }
                    DebugStatus(Status.RUNNING);
                    return Status.RUNNING;
            }
            return Status.RUNNING;
              
        }
        
    }

}