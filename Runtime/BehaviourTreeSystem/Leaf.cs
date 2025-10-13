/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing a leaf node in a behaviour tree that executes a specific action or condition.
    /// </summary>
    public class Leaf : Node
    {
        /// <summary>
        /// Delegate representing the method to be executed by the leaf node.
        /// </summary>
        /// <returns></returns>
        public delegate Status Tick();
        private Tick ProcessMethod;

        /// <summary>
        /// Delegate representing the method to be executed by the leaf node with an index parameter.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public delegate Status TickMulty(int index);
        private TickMulty ProcessMethodMulty;
        private int _index;

        public Leaf() {}

        public Leaf(string name, Tick processMethod,int priority = 0)
        {
            this.name = name;
            this.ProcessMethod = processMethod;
            this.priority = priority;
        }

        public Leaf(string name, TickMulty processMethod,int index, int priority = 0)
        {
            this.name = name;
            this.ProcessMethodMulty = processMethod;
            this.priority = priority;
            _index = index;
        }

        /// <summary>
        /// Process the leaf node by executing its associated method.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            Status newStatus;

            if (ProcessMethod != null)
            {
                newStatus = ProcessMethod();
            }
            else if (ProcessMethodMulty != null)
            {
                newStatus = ProcessMethodMulty(_index);
            }
            else
            {
                newStatus = Status.FAILURE;
            }

            DebugStatus(newStatus);
            return status;
        }
    }

}