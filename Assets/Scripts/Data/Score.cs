using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewScore", menuName = "Scriptable Objects/Data/Score")]
    public class Score : ScriptableObject
    {
        public List<int> Value;
    }
}