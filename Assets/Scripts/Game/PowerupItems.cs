using UnityEngine;

namespace Game.Items
{
    public abstract class Item : MonoBehaviour
    {
        public abstract void Use(PlayerController player);
    }

    public class HPItem : Item
    {
        public override void Use(PlayerController player)
        {
            if (player != null)
            {
                player.IncreaseHealth(50);
                Debug.Log("HP recogido: +50 de vida");
            }
            Destroy(gameObject);
        }
    }

   
  
}
