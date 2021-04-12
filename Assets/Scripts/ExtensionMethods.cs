using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using SpotifyAPI.Web;
using UnityEngine;

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
}