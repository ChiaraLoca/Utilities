using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Singleton class representing a global blackboard for storing and retrieving variables used in behaviour trees.
    /// </summary>
    public class Blackboard : MonoBehaviour
    {
        public static Blackboard Instance;

        private Dictionary<string, BaseBlackboardVariable> _variablesDictionary = new Dictionary<string, BaseBlackboardVariable>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Try to get a variable of type T from the blackboard by its key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue<T>(string key, out T value)
        {
            value = default;
            if (_variablesDictionary.TryGetValue(key, out BaseBlackboardVariable baseVar))
            {
                if (baseVar is BlackboardVariable<T> typedVar)
                {
                    value = typedVar.Value;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Get a variable of type T from the blackboard by its key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public BlackboardVariable<T> GetVariable<T>(string key)
        {
            if (_variablesDictionary.TryGetValue(key, out BaseBlackboardVariable baseVar))
                return baseVar as BlackboardVariable<T>;

            return null;
        }

        /// <summary>
        /// Set or add a variable of type T to the blackboard.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable"></param>
        public void SetVariable<T>(BlackboardVariable<T> variable)
        {
            string key = variable.Name;

            if (_variablesDictionary.ContainsKey(key))
                _variablesDictionary[key] = variable;
            else
                _variablesDictionary.Add(key, variable);
        }

        /// <summary>
        /// Log all variables currently stored in the blackboard.
        /// </summary>
        [ContextMenu("Log Variables")]
        protected void LogVariables()
        {
            string txt = "<b>Blackboard Variables</b>\n";
            List<BaseBlackboardVariable> variables = _variablesDictionary.Values.ToList();

            for (int i = 0; i < variables.Count; i++)
            {
                txt += $"{variables[i].Name} ({variables[i].GetType().Name})\n";
            }

            Debug.Log(txt, this);
        }

    }

    



}