using System;
using Logic.Spotify;
using SpotifyAPI.Web;
using UnityEngine;

namespace Services
{
    [CreateAssetMenu(fileName = "NewSpotifyClient", menuName = "Scriptable Objects/Services/Spotify Client")]
    public class SpotifyClient : ScriptableObject
    {
        [Header("Data")] public SpotifyConfiguration SpotifyConfiguration;

        public SpotifyAPI.Web.SpotifyClient Value;

        public async void FromAuthorizationCode(string authorizationCode)
        {
            try
            {
                string clientId = SpotifyConfiguration.ClientConfiguration.ClientID;
                string clientSecret = SpotifyConfiguration.ClientConfiguration.ClientSecret;
                Uri redirectUri = SpotifyConfiguration.ServerConfiguration.Uri;

                SpotifyClientConfig config = SpotifyClientConfig.CreateDefault();
                OAuthClient client = new OAuthClient(config);
                AuthorizationCodeTokenRequest request =
                    new AuthorizationCodeTokenRequest(clientId, clientSecret, authorizationCode, redirectUri);
                AuthorizationCodeTokenResponse tokenResponse = await client.RequestToken(request);

                Value = new SpotifyAPI.Web.SpotifyClient(tokenResponse);
                Debug.Log("Login and client configuration successful.");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}