
public class TimeItem : Item
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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddTime(20);
        }
        Destroy(gameObject);
    }
}
