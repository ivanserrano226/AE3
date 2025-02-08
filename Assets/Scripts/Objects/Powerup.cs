using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Health, Damage, Time }
    public PowerUpType powerUpType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                ApplyEffect(player);
                Destroy(gameObject); // Destruye el objeto tras recogerlo
            }
        }
    }

    private void ApplyEffect(PlayerController player)
    {
        switch (powerUpType)
        {
            case PowerUpType.Health:
                player.IncreaseHealth(50);
                break;

            case PowerUpType.Damage:
                StartCoroutine(player.TemporaryDamageBoost(15f));
                break;

            case PowerUpType.Time:
                GameManager.Instance.AddTime(20);
                break;
        }
    }
}
