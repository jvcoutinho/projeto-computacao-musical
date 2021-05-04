using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class IntegerRenderer : MonoBehaviour
    {
        [Header("Components")]
        public Text Text;

        [Header("Data")]
        public IntegerVariable Integer;

        private void Update()
        {
            Text.text = Integer.Value.ToString();
        }
    }
}