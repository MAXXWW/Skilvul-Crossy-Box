using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] GameObject eaglePrefabs;
    [SerializeField] int spawnZPos = 7;
    [SerializeField] Player player;
    [SerializeField] float timeOut = 5;
    [SerializeField] float timer = 0;
    float playerLastMaxTravel;

    private void SpawnEagle()
    {
        player.enabled = false;
        var position = new Vector3(player.transform.position.x, 1, player.CurrentTravel + spawnZPos);
        var rotation = Quaternion.Euler(0, 180, 0);
        var eagleObject = Instantiate(eaglePrefabs, position, rotation);
        var eagle = eagleObject.GetComponent<Eagle>();
        eagle.SetUpTarget(player);
    }

    private void Update()
    {
        // jika player ada kemajuan
        if (player.MaxTravel != playerLastMaxTravel)
        {
            // maka reset timer
            timer = 0;
            playerLastMaxTravel = player.MaxTravel;
            return;
        }

        // kalau player tidak ada progress jalankan timer
        if (timer < timeOut)
        {
            timer += Time.deltaTime;
            return;
        }

        // kalau sudah time out
        if (player.IsJumping() == false && player.IsDie == false)
        {
            SpawnEagle();

            if (SoundManager.Instance.asSFXDie.mute != true)
            {
                SoundManager.Instance.asSFXDie.PlayDelayed(1f); //sound efek meninggal
            }
        }
    }
}
