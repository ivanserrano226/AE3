using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected List<GameObject> _spawnPrefabs;
    private GameObject _lastSpawnedPrefab;

    protected virtual void Spawn()
    {
        Spawn(transform.position, Quaternion.identity);
    }
    
    protected virtual void Spawn(Vector3 position, Quaternion rotation)
    {
        // Check if a prefab is already spawned at the position
        Collider[] colliders = Physics.OverlapSphere(position, 1.0f);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject == _lastSpawnedPrefab)
            {
                return;
            }   
        }



        // Choose a random prefab to spawn
        int randomIndex = Random.Range(0, _spawnPrefabs.Count);
        GameObject _spawnPrefab = _spawnPrefabs[randomIndex];

        // Instantiate the prefab at the position and rotation
        _lastSpawnedPrefab = Instantiate(_spawnPrefab, position, rotation);
    }
}
