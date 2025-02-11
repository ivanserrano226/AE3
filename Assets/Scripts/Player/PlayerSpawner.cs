using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    [SerializeField] private List<Transform> _spawnPoints;

    void Start()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Count);
        Transform spawnPoint = _spawnPoints[randomIndex];
        Spawn(spawnPoint.position, spawnPoint.rotation);
    }
}