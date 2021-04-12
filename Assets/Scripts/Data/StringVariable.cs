using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewStringVariable", menuName = "Scriptable Objects/Data/Variables/String")]
    public class StringVariable : ScriptableObject
    {
        public string Value;
    }
}