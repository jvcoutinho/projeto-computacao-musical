using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
using Data;
using Events;
using SpotifyAPI.Web;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpotifyClient = Services.SpotifyClient;

namespace Logic
{
    public class SongSelector : MonoBehaviour
    {
        [Header("Data")] public PlaylistVariable SelectedPlaylist;

        public SelectedSongs SelectedSongs;
        public Score Score;
        public int MaximumScore;

        [Header("Events")] public GameEvent OnSongsSelected;
        public GameEvent OnPlayerWonMatch;

        [Header("Services")] public SpotifyClient SpotifyClient;

        [Header("Components")] public AudioSource FirstSongSource;

        public AudioSource SecondSongSource;

        public List<int> PlayerPositions;
        public List<Transform> PlayerSelectionsUI;
        public List<List<int>> PlayerSelections;


        private void Start()
        {
            PlayerPositions = new List<int> {0, 0};
            PlayerSelections = new List<List<int>> {new List<int>(), new List<int>()};

            ChooseSongsAndPlay().Forget();
        }

        private async UniTaskVoid ChooseSongsAndPlay()
        {
            FullPlaylist playlist = await SpotifyClient.Value.Playlists.Get(SelectedPlaylist.Value.Id);

            Paging<PlaylistTrack<IPlayableItem>>? tracksPaging = playlist.Tracks;
            if (tracksPaging == null)
                throw new InvalidDataException();

            List<PlaylistTrack<IPlayableItem>>? tracks = tracksPaging.Items;

            // TODO filter by preview url
            FullTrack firstRandomSong = (FullTrack) tracks.GetRandomElement().Track;
            FullTrack secondRandomSong = (FullTrack) tracks.GetRandomElement().Track;

            FirstSongSource.clip = await firstRandomSong.GetAudioClip();
            SecondSongSource.clip = await secondRandomSong.GetAudioClip();

            SelectedSongs.CorrectSongs = new List<string>
            {
                firstRandomSong.Name,
                secondRandomSong.Name
            };

            // TODO repeated songs
            SelectedSongs.Songs = new List<string>(SelectedSongs.CorrectSongs)
            {
                ((FullTrack) tracks.GetRandomElement().Track).Name,
                ((FullTrack) tracks.GetRandomElement().Track).Name
            };
            SelectedSongs.Songs.Shuffle();

            OnSongsSelected.Raise();

            FirstSongSource.Play();
            SecondSongSource.Play();
        }

        public void MoveDown(int player)
        {
            PlayerPositions[player] = (PlayerPositions[player] + 1) % SelectedSongs.Songs.Count;

            // TODO change ball position
            // Vector3 currentPosition = PlayerSelectionsUI[player].position;
            // PlayerSelectionsUI[player].position =
            //     new Vector3(currentPosition.x, PlayerPositions[player] * 40, currentPosition.z);
        }

        public void MoveUp(int player)
        {
            if (PlayerPositions[player] == 0)
                PlayerPositions[player] = SelectedSongs.Songs.Count - 1;
            else
                PlayerPositions[player]--;
        }

        public void Select(int player)
        {
            if (PlayerSelections[player].Count >= 2)
                return;

            int currentPosition = PlayerPositions[player];
            PlayerSelections[player].Add(currentPosition);
            if (PlayerSelections[player].Count != 2) return;


            bool playerWon = PlayerSelections[player]
                .Select(position => SelectedSongs.Songs[position])
                .Where(SelectedSongs.CorrectSongs.Contains)
                .Count() == 2;

            if (!playerWon) return;
            Score.Value[player]++;
            if (Score.Value[player] >= MaximumScore) OnPlayerWonMatch.Raise();
            SceneManager.LoadScene("Song Play");
        }
    }
}