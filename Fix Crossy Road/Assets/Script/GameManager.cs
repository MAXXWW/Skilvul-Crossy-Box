using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    public PlayerData Data; //baru
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxTerrainRepeat = 3;
    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] TMP_Text highScore; //baru
    private int playerLastMaxTravel;
    int HighScore;

    private void OnEnable()
    {
        Data = DataManager.LoadData();
        // HighScore = Data.score;
    }

    // private void OnDisable()
    // {
    //     if (player.MaxTravel > HighScore)
    //     {
    //         DataManager.SaveData(this);
    //     }
    //     DataManager.SaveData(this);
    // }

    private void Start()
    {
        // setup game over panel
        gameOverPanel.SetActive(false);
        // gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();

        // belakang
        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        // depan
        for (int z = 1; z <= frontDistance; z++)
        {
            var prefabs = GetNextTerrainPrefabs(z);

            // instantiate blocknya
            CreateTerrain(prefabs, z);
        }

        player.SetUp(backDistance, extent);
    }

    private void Update()
    {
        // cek player masih hidup ga?
        if (player.IsDie && gameOverPanel.activeInHierarchy == false)
        {
            StartCoroutine(ShowGameOverPanel());
        }

        // infinite terrain
        if (player.MaxTravel == playerLastMaxTravel)
        {
            return;
        }

        playerLastMaxTravel = player.MaxTravel;

        // bikin yang ke depan
        var randTBPrefabs = GetNextTerrainPrefabs(player.MaxTravel + frontDistance);
        CreateTerrain(randTBPrefabs, player.MaxTravel + frontDistance);

        // hapus yang di belakang
        var lastTB = map[(player.MaxTravel - 1) + backDistance];

        // TerrainBlock lastTb = map[player.MaxTravel + frontDistance];
        // int lastPos = player.MaxTravel;
        // foreach (var (pos, tb) in map)
        // {
        //     if (pos < lastPos)
        //     {
        //         lastPos = pos;
        //         lastTb = tb;
        //     }
        // }

        map.Remove((player.MaxTravel - 1) + backDistance); //hapus dari daftar (hashmap/dictionary)
        Destroy(lastTB.gameObject); //menghilangkan gameobject dari scene

        player.SetUp(player.MaxTravel + backDistance, extent); //setup lagi supaya player ga bisa bergerak ke belakang
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1.5f); //memberikan delay setelah itu yg dibawah dijalankan

        player.enabled = false;

        if (player.MaxTravel > Data.score)
        {
            Data.score = player.MaxTravel;
            DataManager.SaveData(this);
        }

        gameOverText.text = $"{player.MaxTravel}";
        highScore.text = $"High Score : {Data.score}"; //baru
        gameOverPanel.SetActive(true);
    }

    private void CreateTerrain(GameObject prefabs, int zPos)
    {
        var go = Instantiate(prefabs, new Vector3(0, 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
    }

    private GameObject GetNextTerrainPrefabs(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos - 1];
        for (int distance = 2; distance <= maxTerrainRepeat; distance++)
        {
            if (map[nextPos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if (isUniform)
        {
            if (tbRef is Grass)
            {
                return road;
            }
            else
            {
                return grass;
            }
        }

        // penentuan terrain block dengan probabilitas 50%
        return Random.value > 0.5f ? road : grass;
    }
}
