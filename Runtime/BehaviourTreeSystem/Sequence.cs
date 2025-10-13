
/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// A sequence node that processes its children in order until one fails.
    /// </summary>
    public class Sequence : Node
    {
        public Sequence(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Process the sequence node by processing its children in order.
        /// Returns SUCCESS if all children return SUCCESS, otherwise returns FAILURE if any child fails.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
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