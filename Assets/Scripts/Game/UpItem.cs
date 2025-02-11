using Game.Items;
using UnityEngine;

public class UpItem : Item
    {
        public override void Use(PlayerController player)
        {
            if (player != null)
            {
                player.StartCoroutine(player.TemporaryDamageBoost(15f));
                Debug.Log("Poder aumentado por 15 segundos");
            }
            Destroy(gameObject);
        }
    }