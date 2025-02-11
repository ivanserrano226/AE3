using UnityEngine;

public class Boss : Enemy
{
    protected override void Die()
    {
        GameManager.Instance.OnGameOver(GameOverStatus.Victory);
    }
}
