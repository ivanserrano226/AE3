using UnityEngine;

public class BossSpawner : Spawner
{
    void Start()
    {
        GameManager.Instance.OnCountdownFinished += Spawn;
    }

    void OnDisable()
    {
        GameManager.Instance.OnCountdownFinished -= Spawn;
    }
}
