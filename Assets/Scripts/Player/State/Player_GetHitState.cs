using UnityEngine;

public class Player_GetHitState : PlayerStateBase
{
    private float exitTimer;

    public override void Enter()
    {
        if (Player_Controller.Instance.stats.characterData.currentHealth <= 0)
        {
            Player_Controller.Instance.ChangeState(PlayerState.Dead);
            return;
        }

        if (player.preState == PlayerState.GetHit)
        {
            player.ChangeState(PlayerState.Idle);
        }

        player.Animation_Controller.StopAnimmation();
        player.PlayAnimation("GetHit", null, 1, true);
    }

    public override void Update()
    {
        if (Player_Controller.Instance.stats.characterData.currentHealth <= 0)
        {
            Player_Controller.Instance.ChangeState(PlayerState.Dead);
            return;
        }

        exitTimer++;
        if (exitTimer > 19)
        {
            player.GetHit = false;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h == 0 && v == 0)
        {
            player.ChangeState(PlayerState.Idle);
        }
        else if (!player.GetHit)
        {
            player.ChangeState(PlayerState.Idle);
        }
        else if (h != 0 || v != 0)
        {
            player.ChangeState(PlayerState.GetHit);
        }
    }

    public override void Exit()
    {
        exitTimer = 0;
        player.GetHit = false;
    }
}
