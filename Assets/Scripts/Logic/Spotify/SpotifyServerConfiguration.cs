using System;
using UnityEngine;

namespace Logic.Spotify
{
    [Serializable]
    public class SpotifyServerConfiguration
    {
        [SerializeField] private string BaseUri = "http://localhost:{0}/callback";
        public int Port = 5000;

        public Uri Uri
        {
            get
            {
                string uri = string.Format(BaseUri, Port);
                return new Uri(uri);
            }
        }
    }
}