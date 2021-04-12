using Data;
using Events;
using UnityEngine;

namespace Logic
{
    public class Countdown : MonoBehaviour
    {
        [Header("Data")] public int InitialNumber;

        public IntegerVariable CurrentNumber;

        [Header("Events")] public GameEvent CountdownFinished;

        private float elapsedTime;

        private void Start()
        {
            CurrentNumber.Value = InitialNumber;
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            CurrentNumber.Value = Mathf.CeilToInt(InitialNumber - elapsedTime);
            if (CurrentNumber.Value == 0) CountdownFinished.Raise();
        }
    }
}