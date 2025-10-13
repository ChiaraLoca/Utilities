
/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// A selector node that processes its children in a random order.
    /// </summary>
    public class RandomSelector : Node
    {
        private bool _shuffled =false;

        public RandomSelector(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Shuffle the order of child nodes using the Fisher-Yates algorithm.
        /// </summary>
        private void ShuffleChildren()
        {
            var rand = new System.Random();
            int n = children.Count;
            while (n > 1)
            {
                int k = rand.Next(n--);
                var temp = children[n];
                children[n] = children[k];
                children[k] = temp;
            }
        }

        /// <summary>
        /// Process the random selector node by processing its children in a random order.
        /// Shuffles children only once before processing.
        /// Returns SUCCESS if any child returns SUCCESS, otherwise returns FAILURE if all children fail.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            if (!_shuffled)
            {
                ShuffleChildren();
                _shuffled = true;
            }

            Status childStatus;
            switch (children[currentChild].Process())
            {
                case Status.RUNNING: 
                    childStatus = Status.RUNNING; break;
                
                case Status.SUCCESS:
                    currentChild = 0;
                    foreach (Node child in children)
                        child.Reset();
                    _shuffled = false;
                    childStatus = Status.SUCCESS;
                    break;
                
                case Status.FAILURE:
                    currentChild++;
                    if (currentChild >= children.Count)
                    {
                        currentChild = 0;
                        foreach (Node child in children)
                            child.Reset();
                        _shuffled = false;
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