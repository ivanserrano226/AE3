using UnityEngine;

public class TimeItem : Item
{
    protected override void Update()
    {
        base.Update();
    }

    public override void Use()
    {
        GameManager.Instance.AddTime(10);
    }
}
