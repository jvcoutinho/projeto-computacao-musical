using Data;
using UnityEngine;
using UnityEngine.UI;

public class SongListRenderer : MonoBehaviour
{
    public SelectedSongs SelectedSongs;
    public Transform SongItem;

    private void Start()
    {
        foreach (string song in SelectedSongs.Songs)
        {
            Transform songItem = Instantiate(SongItem, transform);
            Text text = songItem.GetChild(0).GetComponent<Text>();
            text.text = song;
        }
    }
}