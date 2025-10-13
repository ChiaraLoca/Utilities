
/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// A selector node that processes its children based on their priority.
    /// </summary>
    public class PrioritisingSelector : Node
    {
        private bool _sorted = false;

        public PrioritisingSelector(string name)
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
        /// Process the prioritising selector node by processing its children based on their priority.
        /// Sorts children by priority only once before processing.
        /// Returns SUCCESS if any child returns SUCCESS, otherwise returns FAILURE if all children fail.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            if (!_sorted)
            {
                SortChildrenByPriority();
                _sorted = true;
            }
            Status childStatus;
            switch (children[currentChild].Process())
            {
                case Status.RUNNING: childStatus = Status.RUNNING; break;
                case Status.SUCCESS:
                    currentChild = 0;
                    foreach (Node child in children) 
                        child.Reset();
                    _sorted = false;
                    childStatus = Status.SUCCESS;
                    break;
                case Status.FAILURE:
                    currentChild++;
                    if (currentChild >= children.Count)
                    {
                        currentChild = 0;
                        foreach (Node child in children) 
                            child.Reset();
                        _sorted = false;
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