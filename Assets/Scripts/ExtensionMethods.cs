using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using SpotifyAPI.Web;
using UnityEngine;
using UnityEngine.Networking;

public static class ExtensionMethods
{
    public async static UniTask<List<SimplePlaylist>> GetCurrentUsersCollaborativePlaylists(this SpotifyClient client)
    {
        Paging<SimplePlaylist> playlists = await client.Playlists.CurrentUsers();
        if (playlists.Items == null)
            throw new InvalidDataException();
        return playlists.Items;
    }

    public static T GetRandomElement<T>(this List<T> list)
    {
        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public async static UniTask<AudioClip> GetAudioClip(this FullTrack track, AudioType type = AudioType.MPEG)
    {
        UnityWebRequest request = await UnityWebRequestMultimedia
            .GetAudioClip(track.PreviewUrl, AudioType.MPEG).SendWebRequest();
        return DownloadHandlerAudioClip.GetContent(request);
    }
}