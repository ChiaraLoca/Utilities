using System.Globalization;
/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing a dynamic prioritising selector node in a behaviour tree.
    /// </summary>
    public class DynamicPrioritisingSelector : Node
    {
        private bool sorted = false;

        public DynamicPrioritisingSelector(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// TODO: Better way to handle current child QUICK SORT?
        /// </summary>
        void SortChildrenByPriority()
        {
            children.Sort((a, b) => a.priority.CompareTo(b.priority));
        }

        /// <summary>
        /// Process the dynamic prioritising selector node by processing its children based on their priority.
        /// Returns SUCCESS if any child returns SUCCESS, otherwise returns FAILURE if all children fail.
        /// When a child succeeds, its priority is increased, and when it fails, its priority is decreased.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            if (!sorted)
            {
                SortChildrenByPriority();
                sorted = true;
            }

            Status childStatus;
            switch (children[currentChild].Process())
            {
                case Status.RUNNING: childStatus = Status.RUNNING; break;
                case Status.SUCCESS:
                    children[currentChild].priority = 1;
                    currentChild = 0;
                    foreach (Node child in children)
                        child.Reset();
                    sorted = false;
                    childStatus = Status.SUCCESS;
                    break;
                case Status.FAILURE:
                    children[currentChild].priority = 10;
                    currentChild++;
                    if (currentChild >= children.Count)
                    {
                        currentChild = 0;
                        foreach (Node child in children)
                            child.Reset();
                        sorted = false;
                        childStatus = Status.FAILURE;
                    }
                    else
                        childStatus = Status.RUNNING;
                    break;
                default:
                    childStatus = Status.RUNNING; break;
            }
            DebugStatus(childStatus);
            return childStatus;
        }
    }
}