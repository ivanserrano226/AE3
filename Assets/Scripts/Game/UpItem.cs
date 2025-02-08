

public class DamageItem : Item
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
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player != null)
        {
            StartCoroutine(player.TemporaryDamageBoost(15f));
        }
        Destroy(gameObject);
    }


}