using System;
using Cysharp.Threading.Tasks;
using Events;
using Services;
using SpotifyAPI.Web.Auth;
using UnityEngine;

namespace Logic.Spotify
{
    public class SpotifyServer : MonoBehaviour
    {
        [Header("Data")] public SpotifyConfiguration SpotifyConfiguration;

        [Header("Services")] public SpotifyClient Client;

        [Header("Events")] public GameEvent OnServerInitialized;

        private string responseCode;

        private void Start()
        {
            responseCode = null;
            StartServer().Forget();
        }

        private async UniTaskVoid StartServer()
        {
            try
            {
                Uri baseUri = SpotifyConfiguration.ServerConfiguration.Uri;
                int port = SpotifyConfiguration.ServerConfiguration.Port;

                EmbedIOAuthServer server = new EmbedIOAuthServer(baseUri, port);
                await server.Start();

                server.AuthorizationCodeReceived += (sender, response) =>
                {
                    server.Stop();
                    server.Dispose();
                    responseCode = response.Code;
                    return null;
                };

                OnServerInitialized.Raise();

                await UniTask.WaitUntil(() => !string.IsNullOrEmpty(responseCode));
                Client.FromAuthorizationCode(responseCode).Forget();
                Destroy(gameObject);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}