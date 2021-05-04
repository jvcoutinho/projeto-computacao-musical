using System.Collections.Generic;
using UnityEngine;

public class SongItem : MonoBehaviour
{
    public List<GameObject> PlayerSelection;

    public void Select(int player)
    {
        PlayerSelection[player].SetActive(true);
    }
}