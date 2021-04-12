using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Data;
using SpotifyAPI.Web;
using UnityEngine;
using UnityEngine.UI;
using SpotifyClient = Services.SpotifyClient;

namespace UI
{
    public class PlaylistsFetcher : MonoBehaviour
    {
        [Header("Services")] public SpotifyClient Client;

        [Header("Data")] public PlaylistVariable SelectedPlaylist;

        [Header("Components")] public Dropdown Dropdown;

        private void Start()
        {
            AddOptions().Forget();
        }

        private void OnDestroy()
        {
            Dropdown.onValueChanged.RemoveAllListeners();
        }

        private async UniTaskVoid AddOptions()
        {
            List<SimplePlaylist> playlists = await Client.Value.GetCurrentUsersCollaborativePlaylists();
            Dropdown.options = playlists.Select(playlist => new Dropdown.OptionData(playlist.Name)).ToList();
            // TODO check null
            SelectedPlaylist.Value = playlists[0];

            Dropdown.onValueChanged.AddListener(index => { SelectedPlaylist.Value = playlists[index]; });
        }
    }
}