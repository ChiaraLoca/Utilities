using UnityEngine;

/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Generic class for a blackboard variable that can store and retrieve values of a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlackboardVariable<T> : BaseBlackboardVariable
    {
        public T Value { get; set; }

        public BlackboardVariable(string name, T initialValue)
        {
            this.Name = name;
            this.Value = initialValue;
        }

        /// <summary>
        /// Get the value of the blackboard variable.
        /// </summary>
        /// <returns></returns>
        public override object GetValue() => Value;

        /// <summary>
        /// Set the value of the blackboard variable, ensuring type safety.
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(object value)
        {
            if (value is T castValue)
                Value = castValue;
            else
                Debug.LogWarning($"Type mismatch setting BlackboardVariable '{Name}'");
        }
    }

    



}