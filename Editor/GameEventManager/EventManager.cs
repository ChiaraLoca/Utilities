using System;
using System.Collections.Generic;

/// 
/// @author: Chiara Locatelli
/// 
/// <summary>
/// Static class to manage game events.
/// </summary>
/// 
namespace Utilities.GameEventManager
{
    public static class EventManager 
    {
        private static readonly Dictionary<Type, Action<IGameEvent>> _events = new Dictionary<Type, Action<IGameEvent>>();
        private static readonly Dictionary<Delegate, Action<IGameEvent>> _eventLookups = new Dictionary<Delegate, Action<IGameEvent>>();
        
        /// <summary>
        /// Add a listener to a specific game event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="evt"></param>
        public static void AddListener<T>(Action<T> evt) where T : IGameEvent
        {
            if (!_eventLookups.ContainsKey(evt))
            {
                Action<IGameEvent> newAction = (e) => evt((T)e);
                _eventLookups[evt] = newAction;

                if (_events.TryGetValue(typeof(T), out Action<IGameEvent> internalAction))
                    _events[typeof(T)] = internalAction += newAction;
                else
                    _events[typeof(T)] = newAction;
            }
        }

        /// <summary>
        /// Remove a listener from a specific game event.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="evt"></param>
        public static void RemoveListener<T>(Action<T> evt) where T : IGameEvent
        {
            if (_eventLookups.TryGetValue(evt, out var action))
            {
                if (_events.TryGetValue(typeof(T), out var tempAction))
                {
                    tempAction -= action;
                    if (tempAction == null)
                        _events.Remove(typeof(T));
                    else
                        _events[typeof(T)] = tempAction;
                }

                _eventLookups.Remove(evt);
            }
        }

        /// <summary>
        /// Broadcast a specific game event to all its listeners.
        /// </summary>
        /// <param name="evt"></param>
        public static void Broadcast(IGameEvent evt)
        {
            if (_events.TryGetValue(evt.GetType(), out var action))
                action.Invoke(evt);
        }

        /// <summary>
        /// Clear all the events and listeners.
        /// </summary>
        public static void Clear()
        {
            _events.Clear();
            _eventLookups.Clear();
        }
    }
}

