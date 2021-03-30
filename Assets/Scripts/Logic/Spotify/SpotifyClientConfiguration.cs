using System;

namespace Logic.Spotify
{
    [Serializable]
    public class SpotifyClientConfiguration
    {
        public string ClientID;
        public string ClientSecret;
        public string[] Scope;
    }
}