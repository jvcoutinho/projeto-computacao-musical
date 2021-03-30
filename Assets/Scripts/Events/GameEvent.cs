using System.Collections.Generic;
using Logic;
using UnityEngine;

namespace Events
{
    [CreateAssetMenu(fileName = "NewGameEvent", menuName = "Scriptable Objects/Events/Game")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners;

        private void Awake()
        {
            listeners = new List<GameEventListener>();
        }

        public void Register(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void Unregister(GameEventListener listener)
        {
            listeners.Remove(listener);
        }

        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--) listeners[i].RaiseResponse();
        }
    }
}