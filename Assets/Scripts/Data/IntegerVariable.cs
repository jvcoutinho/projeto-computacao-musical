using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewIntegerVariable", menuName = "Scriptable Objects/Data/Variables/Integer")]
    public class IntegerVariable : ScriptableObject
    {
        public int Value;
    }
}