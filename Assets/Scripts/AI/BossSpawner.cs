using UnityEngine;

public class BossSpawner : Spawner
{
    void Start()
    {
        GameManager.Instance.OnCountdownFinishedEvent += Spawn;
    }

    void OnDisable()
    {
        GameManager.Instance.OnCountdownFinishedEvent -= Spawn;
    }
}
