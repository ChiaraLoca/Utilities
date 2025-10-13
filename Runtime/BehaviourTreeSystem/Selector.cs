/// 
/// @author: Chiara Locatelli
/// 
namespace BehaviourTreeSystem
{
    /// <summary>
    /// A selector node that processes its children in order until one succeeds.
    /// </summary>
    public class Selector : Node
    {
        public Selector(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Process the selector node by processing its children in order.
        /// Returns SUCCESS if any child returns SUCCESS, otherwise returns FAILURE if all children fail.
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

                case Status.SUCCESS:
                    currentChild = 0;

                    foreach (Node child in children)
                        child.Reset();

                    DebugStatus(Status.SUCCESS);
                    return Status.SUCCESS;

                case Status.FAILURE:
                    currentChild++;
                    if (currentChild >= children.Count)
                    {
                        currentChild = 0;

                        foreach (Node child in children)
                            child.Reset();

                        DebugStatus(Status.FAILURE);
                        return Status.FAILURE;
                    }

                    DebugStatus(Status.RUNNING);
                    return Status.RUNNING;
            }

            return Status.RUNNING;
        }
    }
}