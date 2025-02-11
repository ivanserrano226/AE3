using UnityEngine;

public class HPItem : Item
{

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Use()
    {
        GameManager.Instance.Player.Heal(25);
    }
}
