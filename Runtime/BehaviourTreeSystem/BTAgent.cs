using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// 
/// @author: Chiara Locatelli
/// 
namespace BehaviourTreeSystem
{
    /// <summary>
    /// Class representing an nav mesh agent that uses a behaviour tree for decision-making.
    /// </summary>
    public class BTAgent : MonoBehaviour
    {
        public NavMeshAgent Agent;
        private ActionState _currentState;

        protected BehaviourTree Tree;
        private Node.Status _treeStatus =Node.Status.RUNNING;

        private WaitForSeconds _waitForSeconds;

        private Vector3 _rememberedPosition;

        private Quaternion _startRotation;
        private float _rotatedAngle = 0f;

        protected bool Paused = false;

        public enum ActionState
        {
            IDLE,
            WORKING
        }

        /// <summary>
        /// Initialize the BTAgent, set up the behaviour tree, and start the behaviour processing coroutine.
        /// Override with new void Start and call base.Start() to extend
        /// </summary>
        public void Start()
        {
            Agent = GetComponent<NavMeshAgent>();
            Tree = new BehaviourTree();
            _waitForSeconds = new WaitForSeconds(UnityEngine.Random.Range(0.1f,0.2f));

            StartCoroutine(Behave());
        }

        /// <summary>
        /// Coroutine that continuously processes the behaviour tree at fixed intervals.
        /// </summary>
        /// <returns></returns>
        IEnumerator Behave()
        {
            while (true)
            {
                if(!Paused)
                    _treeStatus = Tree.Process();

                yield return _waitForSeconds;
            }
        }

        /// <summary>
        /// Check if the agent can see a target within a specified distance and angle.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="tag"></param>
        /// <param name="maxDistance"></param>
        /// <param name="maxAngle"></param>
        /// <returns></returns>
        public Node.Status CanSee(Vector3 target,String tag,float maxDistance,float maxAngle)
        {
            Vector3 directionToTarget = target - transform.position;
            float angle = Vector3.Angle(directionToTarget, transform.forward);

            if (angle <= maxAngle || directionToTarget.magnitude <= maxDistance)
            {
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, directionToTarget,out hitInfo ,maxDistance))
                { 
                    if(hitInfo.collider.CompareTag(tag))
                        return Node.Status.SUCCESS;
                    return Node.Status.FAILURE;
                }
            }
            return Node.Status.FAILURE;
        }

        /// <summary>
        /// Make the agent flee from a specified location by a certain distance.
        /// </summary>
        /// <param name="locationFleeFrom"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public Node.Status Flee(Vector3 locationFleeFrom,float distance)
        {
            if (_currentState == ActionState.IDLE)
            {
                _rememberedPosition = transform.position + (transform.position - locationFleeFrom).normalized * distance;
            }
            return GoToLocation(_rememberedPosition);
        }

        /// <summary>
        /// Make the agent move to a specified destination using the NavMeshAgent.
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public Node.Status GoToLocation(Vector3 destination)
        {
            float distance = Vector3.Distance(transform.position, destination);
            if (_currentState == ActionState.IDLE)
            {
                Agent.SetDestination(destination);
                _currentState = ActionState.WORKING;
            }
            else if (Vector3.Distance(Agent.pathEndPosition, destination) >= 2)
            {
                _currentState = ActionState.IDLE;
                return Node.Status.FAILURE;
            }
            else if (distance <= 2)
            {
                _currentState = ActionState.IDLE;
                return Node.Status.SUCCESS;
            }
            return Node.Status.RUNNING;

        }

        /// <summary>
        /// Rotate the agent by a specified number of degrees at a given rotation speed.
        /// </summary>
        /// <param name="degrees"></param>
        /// <param name="rotationSpeed"></param>
        /// <returns></returns>
        public Node.Status RotateDegrees(float degrees,float rotationSpeed = 180f)
        {
            
            if (_currentState == ActionState.IDLE)
            {
                _currentState = ActionState.WORKING;
                _startRotation = transform.rotation;
                _rotatedAngle = 0f;
            }

            float deltaRotation = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up, deltaRotation);
            _rotatedAngle += deltaRotation;

            if (_rotatedAngle >= degrees)
            {  
                transform.rotation = _startRotation;
                _currentState = ActionState.IDLE;
                return Node.Status.SUCCESS;
            }

            return Node.Status.RUNNING;
        }
    }
}