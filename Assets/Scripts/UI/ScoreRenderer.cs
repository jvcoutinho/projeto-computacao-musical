using Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ScoreRenderer : MonoBehaviour
    {
        public int Player;
        public Score Score;

        public Text Text;

        private void Update()
        {
            int playerScore = Score.Value[Player];
            Text.text = playerScore.ToString();
        }
    }
}