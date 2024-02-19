using JKFrame;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    GUARD,      // ����
    PATROL,     // Ѳ��
    CHASE,      // ׷��
    BASEATTACK, // �չ�
    SKILLATTACK,// ���ܹ���
    DEAD        // ����
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

    [LabelText("����")] public float sightRadius;
    [LabelText("����Ƕ�")] public float sightDegree;
    [LabelText("վ׮")] public bool isGuard; // �ǲ���վ׮���͵���
    private float moveSpeed;

    protected GameObject attackTarget;

    [LabelText("������Ч")] public List<AudioClip> attackAudioList;

    [LabelText("�۲�ʱ��")] public float lookAtTime;

    private float remainLookAtTime; // ������ʱ��ʹ��
    private float lastAttackTime; // ������ʱ��ʹ��

    private Quaternion guardRotation; // ��ʼ��ת

    private int currentHealthVolume; // ����Ѫ�� (��Ѫ�������ı�ʱʹ��)
    [LabelText("��ü�¼����Ѫ��")] public float healthRecordTime; // ���ڱ�����ʱȷ�Ϲ���Ŀ��
    private float healthRecordTimer; // ��ʱ��

    [Header("Patrol State")]

    public float patrolRange;
    private Vector3 wayPoint;
    private Vector3 guardPos;

    // bool��϶���
    private bool isWalk;
    private bool isChase;
    private bool isDead;
    private bool playerDead; // �����û��

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
        GetNewWayPoint(); // �Ȼ�ȡһ��·��
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
    /// �л�״̬
    /// </summary>
    private void SwitchStates()
    {
        if (isDead) enemyStates = EnemyStates.DEAD;
        else if (FoundPlayer()) // �������player���л�׷��״̬
        {
            if (enemyStates != EnemyStates.BASEATTACK && enemyStates != EnemyStates.SKILLATTACK)
                enemyStates = EnemyStates.CHASE;
        }
        else if (stats.characterData.currentHealth != currentHealthVolume) // Ѫ���仯
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
    /// �жϷ�Χ���Ƿ������
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
    /// �ж��Ƿ񿴼����
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
    /// �Ƿ�����Ŀ��
    /// </summary>
    public bool IsFacingTarget(Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);

        return dot > Mathf.Cos((sightDegree / 2) * Mathf.Deg2Rad);
    }

    /// <summary>
    /// վ׮״̬
    /// </summary>
    private void GuardState()
    {
        isWalk = false;
        isChase = false;

        if (transform.position != guardPos) // ����վ׮λ��
        {
            isWalk = true;
            agent.isStopped = false;
            agent.destination = guardPos;

            if (Vector3.SqrMagnitude(guardPos - transform.position) <= agent.stoppingDistance)
            {
                isWalk = false;
                transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.05f); // �ص���ʼ��ת
            }
        }
    }

    /// <summary>
    /// Ѳ��״̬
    /// </summary>
    private void PatrolState()
    {
        isWalk = true;
        isChase = false;
        agent.speed = moveSpeed * 0.7f; // ����
        agent.isStopped = false;

        if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0) // ����
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
    /// ׷��״̬
    /// </summary>
    private void ChaseState()
    {
        // ��϶���
        isWalk = false;
        isChase = true;

        agent.speed = moveSpeed;

        if (!RangeHavePlayer())
        {
            // ���ѣ��ص���һ״̬
            isChase = false;
            if (remainLookAtTime > 0) // ����
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
            // ׷��Player
            isChase = true;
            agent.isStopped = false;
            Vector3 target = new Vector3(attackTarget.transform.position.x, 0, attackTarget.transform.position.z);
            Vector3 self = new Vector3(transform.position.x, 0, transform.position.z);
            Quaternion dir = Quaternion.LookRotation(target - self);
            transform.rotation = Quaternion.Lerp(transform.rotation, dir, 0.3f);
            agent.destination = attackTarget.transform.position;
        }

        // ������Χ���򹥻�
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

            // TODO:������ײ�¼�������
            // �����ж�
            stats.isCritical = Random.value < stats.characterData.criticalChance;
            // ִ�й���
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
    /// ����״̬
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
    ///  �۲�Ѫ��
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
    /// Ŀ���ڹ�����Χ�ڣ�
    /// </summary>
    private bool TargetInAttackRange()
    {
        if (attackTarget != null)
            return Vector3.Distance(attackTarget.transform.position, transform.position) <= stats.characterData.baseAttackRange;
        else return false;
    }

    /// <summary>
    /// Ŀ���ڼ��ܷ�Χ�ڣ�
    /// </summary>
    //private bool TargetInSkillRange()
    //{
    //    if (attackTarget != null)
    //        return Vector3.Distance(attackTarget.transform.position, transform.position) <= characterData.;
    //    else return false;
    //}

    /// <summary>
    /// Ѳ��״̬����ȡ�µ�Ѳ�ߵ�
    /// </summary>
    private void GetNewWayPoint()
    {
        remainLookAtTime = lookAtTime;

        float randomX = Random.Range(-patrolRange, patrolRange);
        float randomZ = Random.Range(-patrolRange, patrolRange);

        // ���ڳ�ʼλ�ã��ƶ���Χ��Ѳ��
        Vector3 randomPoint = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        // ��ֹ����ĵ�λ�޷����ʹ�ò����ж�
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
    /// �չ�����
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
    /// ������Ч
    /// </summary>
    private void AttackAudioShoot()
    {
        int audioIndex = Random.Range(0, attackAudioList.Count);
        AudioSystem.PlayOneShot(attackAudioList[audioIndex], transform.position);
    }
    #endregion

}
