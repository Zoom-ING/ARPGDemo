using JKFrame;
using UnityEngine;
/// <summary>
/// 玩家移动状态
/// </summary>
public class Player_SkillState : PlayerStateBase
{
    private SkillConfig skillConfig;
    // private int baseAttackIndex; // 连击普攻分别在技能配备表中的索引 为 1,2,3...
    private bool isInBaseAttack; // 是否在普通攻击状态下
    private float AttackTimer; // 攻击时间计时器 （帧率 * 帧数）
    private bool whetherBlock; // 阻断连击索引无限增加 阻断代表触发连击了
    private int needDetectionCount; // 伤害检测需要检测的次数，例如持续伤害检测多次，单次伤害检测一次。
    private CharacterStats stats => Player_Controller.Instance.stats;

    public override void Init(IStateMachineOwner owner)
    {
        base.Init(owner);
    }

    public override void Enter()
    {
        if (Player_Controller.Instance.stats.characterData.currentHealth <= 0)
        {
            //Player_Controller.Instance.ChangeState(PlayerState.Dead);
            return;
        }

        if (player.GetHit)
        {
            player.ChangeState(PlayerState.GetHit);
            //player.BaseAttackIndex = 0;
            return;
        }

        AttackTimer = 0;
        whetherBlock = false;
        isInBaseAttack = false;
        //skillConfig = ResSystem.LoadAsset<SkillConfig>(DataManager.PlayerDataStats.SkillStatsData.EquipedSkillDataDic.Dictionary[Player_Controller.Instance.BaseAttackIndex].SkillConfigName);
        skillConfig = ResSystem.LoadAsset<SkillConfig>(Player_Controller.Instance.CurrentSkill.SkillConfigName);
        needDetectionCount = skillConfig.DamageCount;

        if (!isInBaseAttack)
        {
            if (skillConfig.SkillType == SkillType.Base)
            {
                isInBaseAttack = true;
                //Player_Controller.Instance.CurrentSkill = DataManager.PlayerDataStats.SkillStatsData.EquipedSkillDataDic.Dictionary[Player_Controller.Instance.BaseAttackIndex];
            }

        }

        PlaySkill(Player_Controller.Instance.CurrentSkill);
    }

    public override void Update()
    {
        if (Player_Controller.Instance.stats.characterData.currentHealth <= 0)
        {
            //Player_Controller.Instance.ChangeState(PlayerState.Dead);
            return;
        }

        if (player.GetHit)
        {
            player.ChangeState(PlayerState.GetHit);
            player.BaseAttackIndex = 0;
            return;
        }

        AttackTimer -= Time.deltaTime;

        if (isInBaseAttack && !whetherBlock && AttackTimer > 0 && Input.GetMouseButtonDown(0))
        {
            Player_Controller.Instance.BaseAttackIndex++;

            if (Player_Controller.Instance.BaseAttackIndex == 3) // 限制连击段数
            {
                Player_Controller.Instance.BaseAttackIndex = 0;
            }

            whetherBlock = true;
            //Player_Controller.Instance.CurrentSkill = DataManager.PlayerDataStats.SkillStatsData.EquipedSkillDataDic.Dictionary[Player_Controller.Instance.BaseAttackIndex];
        }
    }

    private void PlaySkill(SkillData skillData)
    {
        skillConfig = ResSystem.LoadAsset<SkillConfig>(skillData.SkillConfigName);
        AttackTimer = skillConfig.FrameRote * skillConfig.FrameCount;
        player.Skill_Player.PlaySkill(skillConfig, OnSkillEnd, OnWeaponDetection, OnRootMotion);
        //Debug.Log(skillConfig.SkillName);
    }

    private void OnWeaponDetection(Collider collider)
    {
        if (needDetectionCount == 0) return; // 检测完了不执行

        needDetectionCount--;

        if (collider.tag == "Enemy")
        {
            int damage = Random.Range(skillConfig.MinDamage, skillConfig.MaxDamage);
            stats.TakeDamage(damage, collider.GetComponent<CharacterStats>());
            //Debug.Log(collider.gameObject.name + collider.GetComponent<CharacterStats>().characterData.currentHealth);
        }
    }

    private void OnRootMotion(Vector3 deltaPosition, Quaternion deltaRotation)
    {
        //Debug.Log(deltaPosition.y);
        deltaPosition.y -= 4.8f * Time.deltaTime;
        player.CharacterController.Move(deltaPosition);
        player.ModelTransform.rotation *= deltaRotation;
    }
    private void OnSkillEnd()
    {
        if (whetherBlock) // TODO:阻断代表了普攻连击，因为直接刷新技能状态会出现错误（技能播放器出错），所以暂时先回到Idle状态，优先判断是否触发连击重新回到技能状态。最好是连击了就刷新技能状态
            Player_Controller.Instance.NeedDoubleAttack = true;
        else
        {
            Player_Controller.Instance.BaseAttackIndex = 0;
            Player_Controller.Instance.NeedDoubleAttack = false;
        }

        player.ChangeState(PlayerState.Idle);
    }

    public override void Exit()
    {
        base.Exit();
    }
}