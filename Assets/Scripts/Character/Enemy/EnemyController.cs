using JKFrame;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    GUARD,      // 警戒
    PATROL,     // 巡逻
    CHASE,      // 追逐
    BASEATTACK, // 普攻
    SKILLATTACK,// 技能攻击
    DEAD        // 死亡
}

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private CharacterStats stats;

    private EnemyStates enemyStates;

    private NavMeshAgent agent;

    private Animator animator;

    public GameObject weapon;

    private new Collider collider;

    [Header("Basic Settings")]

    [LabelText("视域")] public float sightRadius;
    [LabelText("视域角度")] public float sightDegree;
    [LabelText("站桩")] public bool isGuard; // 是不是站桩类型敌人
    private float moveSpeed;

    protected GameObject attackTarget;

    [LabelText("攻击音效")] public List<AudioClip> attackAudioList;

    [LabelText("观察时间")] public float lookAtTime;

    private float remainLookAtTime; // 逗留计时器使用
    private float lastAttackTime; // 攻击计时器使用

    private Quaternion guardRotation; // 初始旋转

    private int currentHealthVolume; // 现有血量 (当血量发生改变时使用)
    [LabelText("多久记录现有血量")] public float healthRecordTime; // 用于被攻击时确认攻击目标
    private float healthRecordTimer; // 计时器

    [Header("Patrol State")]

    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPos;

    // bool配合动画
    private bool isWalk;
    private bool isChase;
    private bool isDead;
    private bool playerDead; // 玩家死没死

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        moveSpeed = agent.speed;
        guardPos = transform.position;
        guardRotation = transform.rotation;
        remainLookAtTime = lookAtTime;
        healthRecordTimer = healthRecordTime;
        GetNewWayPoint(); // 先获取一个路点
    }

    private void Start()
    {
        currentHealthVolume = stats.characterData.currentHealth;
        if (isGuard)
        {
            enemyStates = EnemyStates.GUARD;
        }
        else
        {
            enemyStates = EnemyStates.PATROL;
            //GetNewWayPoint();
        }
    }

    private void Update()
    {
        if (stats.characterData.currentHealth <= 0) isDead = true;

        if (!playerDead)
        {
            SwitchStates();
            SwitchAnimation();
            HealthObserve();
            lastAttackTime -= Time.deltaTime;
        }
    }

    private void SwitchAnimation()
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Chase", isChase);
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    private void SwitchStates()
    {
        if (isDead) enemyStates = EnemyStates.DEAD;
        else if (FoundPlayer()) // 如果发现player，切换追击状态
        {
            if (enemyStates != EnemyStates.BASEATTACK && enemyStates != EnemyStates.SKILLATTACK)
                enemyStates = EnemyStates.CHASE;
        }
        else if (stats.characterData.currentHealth != currentHealthVolume) // 血量变化
        {
            if (Player_Controller.Instance != null)
                attackTarget = Player_Controller.Instance.gameObject;
            enemyStates = EnemyStates.CHASE;
        }

        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                GuardState();
                break;
            case EnemyStates.PATROL:
                PatrolState();
                break;
            case EnemyStates.CHASE:
                ChaseState();
                break;
            case EnemyStates.BASEATTACK:
                BaseAttackState();
                break;
            case EnemyStates.SKILLATTACK:
                SkillAttackState();
                break;
            case EnemyStates.DEAD:
                DeadState();
                break;
        }
    }

    /// <summary>
    /// 判断范围内是否有玩家
    /// </summary>
    private bool RangeHavePlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    /// <summary>
    /// 判断是否看见玩家
    /// </summary>
    private bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return IsFacingTarget(transform, attackTarget.transform);
            }
        }

        attackTarget = null;
        return false;
    }

    /// <summary>
    /// 是否面向目标
    /// </summary>
    public bool IsFacingTarget(Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);

        return dot > Mathf.Cos((sightDegree / 2) * Mathf.Deg2Rad);
    }

    /// <summary>
    /// 站桩状态
    /// </summary>
    private void GuardState()
    {
        isWalk = false;
        isChase = false;

        if (transform.position != guardPos) // 不在站桩位置
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;

            if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
            {
                isWalk = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.05f); // 回到初始旋转
            }
        }
    }

    /// <summary>
    /// 巡逻状态
    /// </summary>
    private void PatrolState()
    {
        isWalk = true;
        isChase = false;
        agent.speed = moveSpeed * 0.7f; // 减速
        agent.isStopped = false;

        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0) // 逗留
                remainLookAtTime -= Time.deltaTime;
            else
                GetNewWayPoint();
        }
        else
        {
            isWalk = true;
            agent.destination = wayPoint;
        }
    }

    /// <summary>
    /// 追逐状态
    /// </summary>
    private void ChaseState()
    {
        // 配合动画
        isWalk = false;
        isChase = true;

        agent.speed = moveSpeed;

        if (!RangeHavePlayer())
        {
            // 拉脱，回到上一状态
            isChase = false;
            if (remainLookAtTime > 0) // 逗留
            {
                agent.destination = transform.position;
                remainLookAtTime -= Time.deltaTime;
            }
            else if (isGuard)
                enemyStates = EnemyStates.GUARD;
            else
                enemyStates = EnemyStates.PATROL;
        }
        else
        {
            // 追击Player
            isChase = true;
            agent.isStopped = false;
            Vector3 target = new Vector3(attackTarget.transform.position.x, 0, attackTarget.transform.position.z);
            Vector3 self = new Vector3(transform.position.x, 0, transform.position.z);
            Quaternion dir = Quaternion.LookRotation(target - self);
            transform.rotation = Quaternion.Lerp(transform.rotation, dir, 0.3f);
            agent.destination = attackTarget.transform.position;
        }

        // 攻击范围内则攻击
        //if (TargetInAttackRange() || TargetInSkillRange())
        if (TargetInAttackRange())
        {
            enemyStates = EnemyStates.BASEATTACK;
        }
    }

    private void BaseAttackState()
    {
        isWalk = false;
        isChase = false;
        agent.isStopped = true;

        if (attackTarget != null)
            transform.LookAt(attackTarget.transform.position);

        if (lastAttackTime >= 0)
        {
            if (TargetInAttackRange())
                animator.SetBool("BattleIdle", true);
        }
        else
        {
            lastAttackTime = stats.characterData.baseAttackCoolDown;

            // TODO:武器碰撞事件，暴击
            // 暴击判断
            stats.isCritical = Random.value < stats.characterData.criticalChance;
            // 执行攻击
            BaseAttack();
        }

        if (!TargetInAttackRange())
        {
            animator.SetBool("BattleIdle", false);
            enemyStates = EnemyStates.CHASE;
        }
    }

    private void SkillAttackState()
    {

    }

    /// <summary>
    /// 死亡状态
    /// </summary>
    private void DeadState()
    {
        collider.enabled = false;
        agent.enabled = false;
        agent.radius = 0;
        Destroy(gameObject, 5);
        animator.SetBool("BattleIdle", false);
        animator.SetBool("Death", isDead);
    }

    /// <summary>
    ///  观察血量
    /// </summary>
    private void HealthObserve()
    {
        healthRecordTimer -= Time.deltaTime;
        if (healthRecordTimer < 0)
        {
            currentHealthVolume = stats.characterData.currentHealth;
            healthRecordTimer = healthRecordTime;
        }
    }

    /// <summary>
    /// 目标在攻击范围内？
    /// </summary>
    private bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= stats.characterData.baseAttackRange;
        else return false;
    }

    /// <summary>
    /// 目标在技能范围内？
    /// </summary>
    //private bool TargetInSkillRange()
    //{
    //    if (attackTarget != null)
    //        return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterData.;
    //    else return false;
    //}

    /// <summary>
    /// 巡逻状态，获取新的巡逻点
    /// </summary>
    private void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        // 基于初始位置，移动范围内巡逻
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        // 防止随机的点位无法到达，使用采样判断
        NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomPoint, out hit, patrolRange, 1) ? hit.position : transform.position;
    }

    private void BaseAttack()
    {
        if (attackTarget != null)
            transform.LookAt(attackTarget.transform.position);

        if (TargetInAttackRange())
        {
            if (stats.isCritical)
            {
                animator.SetTrigger("Critical");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
        }
    }


    #region Animation Event

    /// <summary>
    /// 普攻击打
    /// </summary>
    private void BaseAttackHit()
    {
        if (TargetInAttackRange() && IsFacingTarget(transform, attackTarget.transform))
        {
            var targetStats = attackTarget.GetComponent<CharacterStats>();

            targetStats.TakeDamage(stats, targetStats);
            Player_Controller.Instance.GetHit = true;
        }
    }

    /// <summary>
    /// 攻击音效
    /// </summary>
    private void AttackAudioShoot()
    {
        int audioIndex = Random.Range(0, attackAudioList.Count);
        AudioSystem.PlayOneShot(attackAudioList[audioIndex], transform.position);
    }
    #endregion

}
