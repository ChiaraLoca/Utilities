/// 
/// @author: Chiara Locatelli
/// 

namespace BehaviourTreeSystem
{
    /// <summary>
    /// Abstract base class for a blackboard variable that can store and retrieve values of different types.
    /// </summary>
    public abstract class BaseBlackboardVariable
    {
        public string Name { get; protected set; }
        public abstract object GetValue();
        public abstract void SetValue(object value);
    }
}