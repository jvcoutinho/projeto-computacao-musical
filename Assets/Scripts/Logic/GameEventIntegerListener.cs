using Events;
using UnityEngine;
using UnityEngine.Events;

namespace Logic
{
    public class GameEventIntegerListener : MonoBehaviour
    {
        public GameEventInteger Trigger;
        public UnityEvent<int> Response;

        private void OnEnable()
        {
            Trigger.Register(this);
        }

        private void OnDisable()
        {
            Trigger.Unregister(this);
        }

        public void RaiseResponse(int value)
        {
            Response.Invoke(value);
        }
    }
}