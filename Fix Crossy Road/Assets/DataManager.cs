using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static void SaveData(GameManager player)
    {
        PlayerPrefs.SetInt("player_score", player.Data.score);
        PlayerPrefs.Save();
    }

    // Mengembalikan nilai PlayerData makanya tidak menggunakan void
    public static PlayerData LoadData()
    {
        var tmpData = new PlayerData();

        tmpData.score = PlayerPrefs.GetInt("player_score");
        return tmpData;
    }
}
