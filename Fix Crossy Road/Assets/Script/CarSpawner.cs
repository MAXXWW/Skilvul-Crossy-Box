using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] GameObject carPrefabs;
    [SerializeField] TerrainBlock terrain;
    [SerializeField] float minSpawnDuration = 2f;
    [SerializeField] float MaxSpawnDuration = 3f;
    bool isRight;
    float timer;

    private void Start()
    {
        isRight = Random.value > 0.5f ? true : false;
        timer = Random.Range(minSpawnDuration, MaxSpawnDuration);
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        timer = Random.Range(minSpawnDuration, MaxSpawnDuration);

        var spawnPos = this.transform.position + Vector3.right * (isRight ? -(terrain.Extent + 1) : terrain.Extent + 1);
        var go = Instantiate(
        original: carPrefabs,
        position: spawnPos,
        rotation: Quaternion.Euler(0, isRight ? 90 : -90, 0),
        parent: this.transform);

        var car = go.GetComponent<Car>();
        car.SetUp(terrain.Extent);
    }
}
