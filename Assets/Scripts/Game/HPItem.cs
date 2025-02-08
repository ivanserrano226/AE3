using UnityEngine;

public class HP : Item
{
    private void Start()
    {
        // Inicialización específica si es necesario
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void Use()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.IncreaseHealth(50);
        }
        Destroy(gameObject);
    }
}