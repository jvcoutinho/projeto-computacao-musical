using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewSelectedSongs", menuName = "Scriptable Objects/Data/Selected Songs")]
    public class SelectedSongs : ScriptableObject
    {
        public List<string> Songs;
        public List<string> CorrectSongs;
    }
}