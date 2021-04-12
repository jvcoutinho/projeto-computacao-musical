using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using SpotifyAPI.Web;
using UnityEngine;
using UnityEngine.Networking;
using SpotifyClient = Services.SpotifyClient;

namespace Logic
{
    public class SongSelector : MonoBehaviour
    {
        [Header("Data")] public PlaylistVariable SelectedPlaylist;

        [Header("Services")] public SpotifyClient SpotifyClient;

        [Header("Components")] public AudioSource FirstSongSource;

        public AudioSource SecondSongSource;

        private async void Start()
        {
            FullPlaylist playlist = await SpotifyClient.Value.Playlists.Get(SelectedPlaylist.Value.Id);

            List<PlaylistTrack<IPlayableItem>>? tracks = playlist.Tracks!.Items;

            FullTrack firstRandomSong = (FullTrack) tracks.GetRandomElement().Track;
            FullTrack secondRandomSong = (FullTrack) tracks.GetRandomElement().Track;

            UnityWebRequest request = await UnityWebRequestMultimedia
                .GetAudioClip(firstRandomSong.PreviewUrl, AudioType.MPEG).SendWebRequest();
            FirstSongSource.clip = DownloadHandlerAudioClip.GetContent(request);

            UnityWebRequest request2 = await UnityWebRequestMultimedia
                .GetAudioClip(secondRandomSong.PreviewUrl, AudioType.MPEG).SendWebRequest();
            SecondSongSource.clip = DownloadHandlerAudioClip.GetContent(request2);

            FirstSongSource.Play();
            SecondSongSource.Play();
        }
    }
}