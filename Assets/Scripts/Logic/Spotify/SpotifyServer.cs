using System;
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

        private async void Start()
        {
            try
            {
                Uri baseUri = SpotifyConfiguration.ServerConfiguration.Uri;
                int port = SpotifyConfiguration.ServerConfiguration.Port;

                EmbedIOAuthServer server = new EmbedIOAuthServer(baseUri, port);
                await server.Start();

                server.AuthorizationCodeReceived += async (sender, response) =>
                {
                    await server.Stop();
                    Client.FromAuthorizationCode(response.Code);
                    Destroy(gameObject);
                };

                OnServerInitialized.Raise();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}