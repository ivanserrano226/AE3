using UnityEngine;
using System.Collections;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUps; // Array con los prefabs (Pills, Bomb, Timer)
    public Transform[] spawnPoints; // Puntos donde pueden aparecer los objetos
    public float spawnInterval = 10f; // Tiempo entre cada aparición

    private void Start()
    {
        StartCoroutine(SpawnPowerUps());
    }

    private IEnumerator SpawnPowerUps()
    {
        while (true) // Se ejecuta continuamente
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomPowerUp();
        }
    }

    private void SpawnRandomPowerUp()
    {
        if (powerUps.Length == 0 || spawnPoints.Length == 0)
            return;

        // Seleccionar un punto aleatorio del mapa
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Seleccionar un power-up aleatorio
        GameObject powerUpPrefab = powerUps[Random.Range(0, powerUps.Length)];

        // Instanciar el power-up en la posición seleccionada
        Instantiate(powerUpPrefab, spawnPoint.position, Quaternion.identity);
    }
}
