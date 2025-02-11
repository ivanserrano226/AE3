using UnityEngine;

public class TimedSpawner : Spawner
{
    [SerializeField] private float _spawnRate = 15.0f;
    private float _nextSpawnTime = 0f;

    protected virtual void Start()
    {
        Spawn();
    }

    protected virtual void Update() 
    {
        if (Time.time >= _nextSpawnTime)
        {
            _nextSpawnTime = Time.time + _spawnRate;
            Spawn();
        }
    }
}
