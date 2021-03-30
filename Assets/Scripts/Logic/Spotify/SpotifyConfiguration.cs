using UnityEngine;

namespace Logic.Spotify
{
    [CreateAssetMenu(fileName = "NewSpotifyConfiguration", menuName = "Scriptable Objects/Data/Spotify Configuration")]
    public class SpotifyConfiguration : ScriptableObject
    {
        public SpotifyClientConfiguration ClientConfiguration;
        public SpotifyServerConfiguration ServerConfiguration;
    }
}