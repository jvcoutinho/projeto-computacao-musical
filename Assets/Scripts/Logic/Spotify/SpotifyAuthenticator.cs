using System;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using UnityEngine;

namespace Logic.Spotify
{
    public class SpotifyAuthenticator : MonoBehaviour
    {
        public SpotifyConfiguration SpotifyConfiguration;

        public void AuthenticateUser()
        {
            Uri baseUri = SpotifyConfiguration.ServerConfiguration.Uri;
            string clientId = SpotifyConfiguration.ClientConfiguration.ClientID;

            LoginRequest request = new LoginRequest(baseUri, clientId, LoginRequest.ResponseType.Code)
            {
                Scope = SpotifyConfiguration.ClientConfiguration.Scope
            };
            BrowserUtil.Open(request.ToUri());
        }
    }
}