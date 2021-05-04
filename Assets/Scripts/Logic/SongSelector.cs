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
        [Header("Data")]
        public PlaylistVariable SelectedPlaylist;

        public SelectedSongs SelectedSongs;
        public Score Score;
        public int MaximumScore;

        [Header("Events")]
        public GameEvent OnSongsSelected;

        public GameEvent OnPlayerWonMatch;

        [Header("Services")]
        public SpotifyClient SpotifyClient;

        [Header("Components")]
        public AudioSource FirstSongSource;

        public AudioSource SecondSongSource;

        public List<int> PlayerPositions;
        public Transform SongListUI;
        public float PlayerSelectionsUIInitialPosition;

        public List<RectTransform> PlayerSelectionsUI;
        public List<List<int>> PlayerSelections;

        private void Start()
        {
            PlayerPositions = new List<int> {0, 0};
            PlayerSelections = new List<List<int>> {new List<int>(), new List<int>()};

            ChooseSongsAndPlay().Forget();
        }

        private async UniTaskVoid ChooseSongsAndPlay()
        {
            // Fetch selected playlist tracks.
            FullPlaylist playlist = await SpotifyClient.Value.Playlists.Get(SelectedPlaylist.Value.Id);

            Paging<PlaylistTrack<IPlayableItem>>? tracksPaging = playlist.Tracks;
            if (tracksPaging == null)
                throw new InvalidDataException();

            List<PlaylistTrack<IPlayableItem>>? tracks = tracksPaging.Items;

            // Fetch two random songs.
            // TODO filter by sample
            FullTrack firstRandomSong = (FullTrack) tracks.GetRandomElement().Track;
            FullTrack secondRandomSong = (FullTrack) tracks.GetRandomElement().Track;

            FirstSongSource.clip = await firstRandomSong.GetAudioClip();
            SecondSongSource.clip = await secondRandomSong.GetAudioClip();

            SelectedSongs.CorrectSongs = new List<string>
            {
                firstRandomSong.Name,
                secondRandomSong.Name
            };

            // Define correct songs (the ones that will play).
            SelectedSongs.Songs = new List<string>(SelectedSongs.CorrectSongs)
            {
                ((FullTrack) tracks.GetRandomElement().Track).Name,
                ((FullTrack) tracks.GetRandomElement().Track).Name
            };
            SelectedSongs.Songs.Shuffle();

            TrackAudioAnalysis firstSongAnalysis =
                await SpotifyClient.Value.Tracks.GetAudioAnalysis(firstRandomSong.Id);
            TrackAudioAnalysis secondSongAnalysis =
                await SpotifyClient.Value.Tracks.GetAudioAnalysis(firstRandomSong.Id);

            Segment firstSongRandomSegment = firstSongAnalysis.Segments.GetRandomElement();
            Segment secondSongRandomSegment = secondSongAnalysis.Segments.GetRandomElement();

            FirstSongSource.pitch =
                secondSongRandomSegment.Pitches.Count > 0 ? secondSongRandomSegment.Pitches.Average() : 1f;
            SecondSongSource.pitch =
                firstSongRandomSegment.Pitches.Count > 0 ? firstSongRandomSegment.Pitches.Average() : 1f;

            FirstSongSource.pitch = Mathf.Clamp(FirstSongSource.pitch, 0.5f, 1f);
            SecondSongSource.pitch = Mathf.Clamp(SecondSongSource.pitch, 0.5f, 1f);

            // Play the songs.
            FirstSongSource.Play();
            SecondSongSource.Play();

            OnSongsSelected.Raise();
        }

        public void OnMoveDown_1()
        {
            MoveDown(0);
        }

        public void OnMoveDown_2()
        {
            MoveDown(1);
        }

        public void OnMoveUp_1()
        {
            MoveUp(0);
        }

        public void OnMoveUp_2()
        {
            MoveUp(1);
        }

        public void OnSelect_1()
        {
            Select(0);
        }

        public void OnSelect_2()
        {
            Select(1);
        }

        private void MoveDown(int player)
        {
            PlayerPositions[player] = (PlayerPositions[player] + 1) % SelectedSongs.Songs.Count;

            // TODO change ball position
            Vector2 currentPosition = PlayerSelectionsUI[player].anchoredPosition;
            PlayerSelectionsUI[player].anchoredPosition =
                new Vector2(currentPosition.x, PlayerSelectionsUIInitialPosition - PlayerPositions[player] * 40);
        }

        private void MoveUp(int player)
        {
            if (PlayerPositions[player] == 0)
                PlayerPositions[player] = SelectedSongs.Songs.Count - 1;
            else
                PlayerPositions[player]--;

            Vector2 currentPosition = PlayerSelectionsUI[player].anchoredPosition;
            PlayerSelectionsUI[player].anchoredPosition =
                new Vector2(currentPosition.x, PlayerSelectionsUIInitialPosition - PlayerPositions[player] * 40);
        }

        private void Select(int player)
        {
            if (PlayerSelections[player].Count >= 2)
                return;

            int currentPosition = PlayerPositions[player];
            PlayerSelections[player].Add(currentPosition);

            var songItemTransform = SongListUI.GetChild(currentPosition);
            SongItem songItem = songItemTransform.GetComponent<SongItem>();
            songItem.Select(player);

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