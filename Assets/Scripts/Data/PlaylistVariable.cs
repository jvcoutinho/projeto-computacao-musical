using SpotifyAPI.Web;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "NewPlaylistVariable", menuName = "Scriptable Objects/Data/Variables/Playlist")]
    public class PlaylistVariable : ScriptableObject
    {
        public SimplePlaylist Value;
    }
}