/// 
/// @author: Chiara Locatelli
/// 
namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing an inverter node in a behaviour tree that inverts the result of its child node.
    /// It has only one child.
    /// </summary>
    public class Inverter : Node
    {
        public Inverter(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Process the inverter node by processing its single child and inverting its result.
        /// Returns SUCCESS if the child returns FAILURE, and FAILURE if the child returns SUCCESS.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            Status childStatus;
            switch (children[0].Process())
            {
                case Status.RUNNING: childStatus = Status.RUNNING; break;
                case Status.SUCCESS: childStatus = Status.FAILURE; break;
                case Status.FAILURE: childStatus = Status.SUCCESS; break;
                default: childStatus = Status.RUNNING; break;
            }

            DebugStatus(childStatus);
            return childStatus;
        }
    }
}