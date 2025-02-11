using Game.Items;
using UnityEngine;

public class TimeItem : Item
    {
        public override void Use(PlayerController player)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddTime(20);
                Debug.Log("Tiempo extra: +20 segundos");
            }
            Destroy(gameObject);
        }
    }
