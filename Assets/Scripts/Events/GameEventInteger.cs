using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "NewGameEventInteger", menuName = "Scriptable Objects/Events/Game (int)")]
    public class GameEventInteger : ScriptableObject
    {
        private List<GameEventIntegerListener> listeners;

        private void Awake()
        {
            listeners = new List<GameEventIntegerListener>();
        }

        public void Register(GameEventIntegerListener listener)
        {
            listeners.Add(listener);
        }

        public void Unregister(GameEventIntegerListener listener)
        {
            listeners.Remove(listener);
        }

        public void Raise(int value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].RaiseResponse(value);
        }
    }
}