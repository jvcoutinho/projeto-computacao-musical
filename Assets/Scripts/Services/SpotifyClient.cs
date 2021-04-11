using System;
using Cysharp.Threading.Tasks;
using Events;
using Logic.Spotify;
using SpotifyAPI.Web;
using UnityEngine;

namespace Services
{
    [CreateAssetMenu(fileName = "NewSpotifyClient", menuName = "Scriptable Objects/Services/Spotify Client")]
    public class SpotifyClient : ScriptableObject
    {
        [Header("Data")] public SpotifyConfiguration SpotifyConfiguration;

        [Header("Events")] public GameEvent OnAuthenticationCompleted;

        public SpotifyAPI.Web.SpotifyClient Value;

        public async UniTaskVoid FromAuthorizationCode(string authorizationCode)
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

                OnAuthenticationCompleted.Raise();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}