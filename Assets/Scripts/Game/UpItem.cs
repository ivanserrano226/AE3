public class UpItem : Item
{
    protected override void Update()
    {
        base.Update();
    }

    public override void Use()
    {
        GameManager.Instance.Player.DamagePowerUp();
    }
}