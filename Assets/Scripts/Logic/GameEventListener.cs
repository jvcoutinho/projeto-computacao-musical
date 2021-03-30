using Events;
using UnityEngine;
using UnityEngine.Events;

namespace Logic
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Trigger;
        public UnityEvent Response;

        private void OnEnable()
        {
            Trigger.Register(this);
        }

        private void OnDisable()
        {
            Trigger.Unregister(this);
        }

        public void RaiseResponse()
        {
            Response.Invoke();
        }
    }
}