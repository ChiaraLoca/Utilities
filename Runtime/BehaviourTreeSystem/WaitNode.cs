using UnityEngine;

/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing a wait node in a behaviour tree that waits for a specified duration before succeeding.
    /// </summary>
    public class WaitNode : Node
    {
        private float waitTime;
        private float startTime;
        private bool started = false;

        public WaitNode(string name, float waitTime)
        {
            this.name = name;
            this.waitTime = waitTime;
        }

        /// <summary>
        /// Process the wait node by checking if the specified wait time has elapsed.
        /// Returns RUNNING if still waiting, otherwise returns SUCCESS.
        /// </summary>
        /// <returns></returns>
        public override Status Process()
        {
            if (!started)
            {
                started = true;
                startTime = Time.time;  
            }

            float elapsed = Time.time - startTime;
            Status currentStatus;
            if (elapsed < waitTime)
            {
                currentStatus = Status.RUNNING;
            }
            else
            {
                started = false;
                currentStatus = Status.SUCCESS;
            }

            DebugStatus(currentStatus);
            return currentStatus;
        }

        
    }

    



}