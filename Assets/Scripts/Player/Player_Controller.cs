using JKFrame;
using System;
using UnityEngine;

public class Player_Controller : SingletonMono<Player_Controller>, IStateMachineOwner
{

    [Header("Base Setting")]
    private bool canControl = true; // 能否操控角色释放技能(不影响闲置和移动)
    public bool CanControl { get => canControl; set => canControl = value; }

    [Header("Component")]
    [SerializeField] private Player_View view;
    [SerializeField] private Skill_Player skill_Player;
    public Skill_Player Skill_Player { get => skill_Player; }
    [SerializeField] private CharacterController characterController;
    public CharacterController CharacterController { get => characterController; }

    [Header("Other")]
    private StateMachine stateMachine;
    private PlayerState playerState; // 玩家的当前状态
    public PlayerState preState; // 玩家的上一状态
    private CharacterConfig characterConfig;
    public CharacterConfig CharacterConfig { get => characterConfig; }
    public Animation_Controller Animation_Controller { get => view.Animation; }
    public Transform ModelTransform { get => view.transform; }
    public float WalkSpeed { get => characterConfig.WalkSpeed; }
    public float RunSpeed { get => characterConfig.RunSpeed; }
    public float RotateSpeed { get => characterConfig.RotateSpeed; }

    [Header("Skill")]
    [HideInInspector] public SkillData CurrentSkill;
    [HideInInspector] public int BaseAttackIndex; // 连击段数索引
    [HideInInspector] public bool NeedDoubleAttack; // 触发连击了

    [Header("Combat")]
    public CharacterStats stats;
    public bool GetHit;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {
        // TODO:之后根据不同职业，获取不同的角色配置
        characterConfig = ResSystem.LoadAsset<CharacterConfig>("WarriorConfig");
        view.InitOnGame(DataManager.CustomCharacterData);
        skill_Player.Init(view.Animation, ModelTransform);
        // 初始化状态机
        stateMachine = ResSystem.GetOrNew<StateMachine>();
        stateMachine.Init(this);
        // 默认状态为待机
        ChangeState(PlayerState.Idle);

        // TODO：初始化技能数据
        //InitPlayerSkillData();
    }

    private void Update()
    {

    }

    /// <summary>
    /// 修改状态
    /// </summary>
    public void ChangeState(PlayerState playerState, bool reCurrState = false)
    {
        this.preState = this.playerState;
        this.playerState = playerState;
        switch (playerState)
        {
            case PlayerState.Idle:
                stateMachine.ChangeState<Player_IdleState>(reCurrState);
                break;
            case PlayerState.Move:
                stateMachine.ChangeState<Player_MoveState>(reCurrState);
                break;
            case PlayerState.Skill:
                stateMachine.ChangeState<Player_SkillState>(reCurrState);
                break;
            case PlayerState.GetHit:
                stateMachine.ChangeState<Player_GetHitState>(reCurrState);
                break;
            case PlayerState.Dead:
                stateMachine.ChangeState<Player_DeadState>(reCurrState);
                break;
        }
    }

    /// <summary>
    /// 确定施放哪个技能，以及技能释放出去
    /// </summary>
    public void ConfirmReleaseSkill()
    {
        if (!CanControl) return;

        //if (Input.GetMouseButtonDown(0) || NeedDoubleAttack) // 鼠标左键 和 触发了连击
        if (Input.GetMouseButtonDown(0) && CanControl || NeedDoubleAttack && CanControl) // 鼠标左键 和 触发了连击
        {
            CurrentSkill = DataManager.PlayerDataStats.SkillStatsData.EquipedSkillDataDic.Dictionary[BaseAttackIndex]; // 普攻第一段
            ChangeState(PlayerState.Skill);
        }
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    public void PlayAnimation(string animationClipName, Action<Vector3, Quaternion> rootMotionAction = null, float speed = 1, bool refreshAnimation = false, float transitionFixedTime = 0.25f)
    {
        if (rootMotionAction != null)
        {
            view.Animation.SetRootMotionAction(rootMotionAction);
        }
        view.Animation.PlaySingleAniamtion(characterConfig.GetAnimationByName(animationClipName), speed, refreshAnimation, transitionFixedTime);
    }

    /// <summary>
    /// 播放混合动画
    /// </summary>
    public void PlayBlendAnimation(string clip1Name, string clip2Name, Action<Vector3, Quaternion> rootMotionAction = null, float speed = 1, float transitionFixedTime = 0.25f)
    {
        if (rootMotionAction != null)
        {
            view.Animation.SetRootMotionAction(rootMotionAction);
        }
        AnimationClip clip1 = characterConfig.GetAnimationByName(clip1Name);
        AnimationClip clip2 = characterConfig.GetAnimationByName(clip2Name);
        view.Animation.PlayBlendAnimation(clip1, clip2, speed, transitionFixedTime);
    }


    public void StopAnimation()
    {
        view.Animation.StopAnimmation();
    }

    private void OnDestroy()
    {
        stateMachine.Stop();
        stateMachine = null;
    }
}
