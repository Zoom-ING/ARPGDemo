using JKFrame;
using UnityEngine;

/// <summary>
/// 玩家待机状态
/// </summary>
public class Player_IdleState : PlayerStateBase
{
    private CharacterController characterController;

    public override void Init(IStateMachineOwner owner)
    {
        base.Init(owner);
    }

    public override void Enter()
    {
        if (Player_Controller.Instance.stats.characterData.currentHealth <= 0)
        {
            Player_Controller.Instance.ChangeState(PlayerState.Dead);
            return;
        }

        if (player.GetHit)
        {
            player.ChangeState(PlayerState.GetHit);
            return;
        }


        if (Player_Controller.Instance.NeedDoubleAttack) return; // 检测是否普攻连击

        // 播放待机动作
        player.PlayAnimation("Idle");
    }

    public override void Update()
    {
        if (Player_Controller.Instance.stats.characterData.currentHealth <= 0)
        {
            Player_Controller.Instance.ChangeState(PlayerState.Dead);
            return;
        }

        if (player.GetHit)
        {
            player.ChangeState(PlayerState.GetHit);
            return;
        }

        player.CharacterController.Move(new Vector3(0, -9.8f * Time.deltaTime, 0));
        // 检测玩家的输入
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h != 0 || v != 0)
        {
            // 切换状态
            player.ChangeState(PlayerState.Move);
        }

        // TODO:测试进入技能
        Player_Controller.Instance.ConfirmReleaseSkill();
    }

    ///// <summary>
    ///// 确定施放哪个技能，以及技能释放出去
    ///// </summary>
    //public void ConfirmReleaseSkill()
    //{
    //    if (Input.GetMouseButtonDown(0) && Player_Controller.Instance.CanControl)
    //    {
    //        Player_Controller.Instance.CurrentSkill = DataManager.PlayerDataStats.SkillStatsData.EquipedSkillDataDic.Dictionary[0]; // 普攻第一段
    //        player.ChangeState(PlayerState.Skill);
    //    }
    //}
}
