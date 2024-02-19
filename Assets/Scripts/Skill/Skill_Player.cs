using JKFrame;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能播放器
/// </summary>
public class Skill_Player : SerializedMonoBehaviour
{
    private Animation_Controller animation_Controller;
    private bool isPlaying = false;     // 当前是否处于播放状态
    public bool IsPlaying
    {
        get => isPlaying;
    }

    private SkillConfig skillConfig;    // 当前播放的技能配置
    private int currentFrameIndex;      // 当前是第几帧
    private float playTotalTime;        // 当前播放的总时间
    private int frameRote;              // 当前技能的帧率

    private Transform modelTransform;
    public Transform ModelTransform { get => modelTransform; }
    public LayerMask attackDetectionLayer;
    public void Init(Animation_Controller animation_Controller, Transform modelTransform)
    {
        this.animation_Controller = animation_Controller;
        this.modelTransform = modelTransform;
        foreach (Skill_Weapon item in WeaponDic.Values)
        {
            item.Init(attackDetectionLayer, OnWeaponDetection);
        }
    }
    #region 武器
    [SerializeField] private Dictionary<string, Skill_Weapon> weaponDic = new Dictionary<string, Skill_Weapon>();
    public Dictionary<string, Skill_Weapon> WeaponDic { get => weaponDic; }

    private void OnWeaponDetection(Collider other)
    {
        onWeaponDetection?.Invoke(other);
    }

    #endregion
    private Action<Vector3, Quaternion> rootMotionAction;
    private Action skillEndAction;
    private Action<Collider> onWeaponDetection;

    /// <summary>
    /// 播放技能
    /// </summary>
    /// <param name="skillConfig">技能配置</param>
    public void PlaySkill(SkillConfig skillConfig, Action skillEndAction, Action<Collider> onWeaponDetection, Action<Vector3, Quaternion> rootMotionAction = null)
    {
        this.skillConfig = skillConfig;
        currentFrameIndex = -1;
        frameRote = skillConfig.FrameRote;
        playTotalTime = 0;
        isPlaying = true;
        this.skillEndAction = skillEndAction;
        this.rootMotionAction = rootMotionAction;
        this.onWeaponDetection = onWeaponDetection;
        TickSkill();
    }

    private void Clean()
    {
        if (rootMotionAction != null) animation_Controller.ClearRootMotionAction();
        this.skillEndAction = null;
        this.rootMotionAction = null;
        this.onWeaponDetection = null;
        skillConfig = null;
    }

    private void Update()
    {
        if (isPlaying)
        {
            playTotalTime += Time.deltaTime;
            // 根据总时间判断当前是第几帧
            int targetFrameIndex = (int)(playTotalTime * frameRote);
            // 防止一帧延迟过大，追帧
            while (currentFrameIndex < targetFrameIndex)
            {
                // 驱动一次技能
                TickSkill();
            }
            // 如果到达最后一帧，技能结束
            if (targetFrameIndex >= skillConfig.FrameCount)
            {
                isPlaying = false;
                skillEndAction?.Invoke();
                Clean();
            }
        }
    }

    private void TickSkill()
    {
        currentFrameIndex += 1;
        // 驱动动画
        if (animation_Controller != null && skillConfig.SkillAnimationData.FrameData.TryGetValue(currentFrameIndex, out SkillAnimationEvent skillAnimationEvent))
        {
            animation_Controller.PlaySingleAniamtion(skillAnimationEvent.AnimationClip, 1, true, skillAnimationEvent.TransitionTime);

            if (skillAnimationEvent.ApplyRootMotion)
            {
                animation_Controller.SetRootMotionAction(rootMotionAction);
            }
            else
            {
                animation_Controller.ClearRootMotionAction();
            }
        }

        // 驱动音效
        for (int i = 0; i < skillConfig.SkillAudioData.FrameData.Count; i++)
        {
            SkillAudioEvent audioEvent = skillConfig.SkillAudioData.FrameData[i];
            if (audioEvent.AudioClip != null && audioEvent.FrameIndex == currentFrameIndex)
            {
                // 播放音效，从头播放
                AudioSystem.PlayOneShot(audioEvent.AudioClip, transform.position, false, audioEvent.Voluem);
            }
        }

        // 驱动特效
        for (int i = 0; i < skillConfig.SkillEffectData.FrameData.Count; i++)
        {
            SkillEffectEvent effectEvent = skillConfig.SkillEffectData.FrameData[i];
            if (effectEvent.Prefab != null && effectEvent.FrameIndex == currentFrameIndex)
            {
                // 实例化特效
                GameObject effectObj = PoolSystem.GetGameObject(effectEvent.Prefab.name);
                if (effectObj == null)
                {
                    effectObj = GameObject.Instantiate(effectEvent.Prefab);
                    effectObj.name = effectEvent.Prefab.name;
                }
                effectObj.transform.position = modelTransform.TransformPoint(effectEvent.Position);
                effectObj.transform.rotation = Quaternion.Euler(modelTransform.eulerAngles + effectEvent.Rotation);
                effectObj.transform.localScale = effectEvent.Scale;
                if (effectEvent.AutoDestruct)
                {
                    StartCoroutine(AutoDestructEffectGameObject((float)effectEvent.Duration / skillConfig.FrameRote, effectObj));
                }
            }
        }

#if UNITY_EDITOR
        if (drawAttackDetectionGizmos) currentAttackDetectionList.Clear();
#endif

        // 驱动伤害监测
        for (int i = 0; i < skillConfig.SkillAttackDetectionData.FrameData.Count; i++)
        {
            SkillAttackDetectionEvent detectionEvent = skillConfig.SkillAttackDetectionData.FrameData[i];
            AttackDetectionType attackDetectionType = detectionEvent.GetAttackDetectionType();
            // 武器需要关注第一帧和结束帧
            if (attackDetectionType == AttackDetectionType.Weapon)
            {
                if (detectionEvent.FrameIndex == currentFrameIndex)
                {
                    //驱动武器检测开启
                    AttackWeaponDetectionData weaponDetectionData = (AttackWeaponDetectionData)detectionEvent.AttackDetectionData;
                    if (weaponDic.TryGetValue(weaponDetectionData.weaponName, out Skill_Weapon weapon))
                    {
                        weapon.StartDetection();
                    }
                    else Debug.LogError("没有指定的武器");
                }
                if (currentFrameIndex == detectionEvent.FrameIndex + detectionEvent.DurationFrame)
                {
                    //驱动武器检测关闭
                    AttackWeaponDetectionData weaponDetectionData = (AttackWeaponDetectionData)detectionEvent.AttackDetectionData;
                    if (weaponDic.TryGetValue(weaponDetectionData.weaponName, out Skill_Weapon weapon))
                    {
                        weapon.StopDetection();
                    }
                    else Debug.LogError("没有指定的武器");
                }
            }
            // 其他形状内每一帧都做检测
            else
            {
                // 当前帧在范围内
                if (currentFrameIndex >= detectionEvent.FrameIndex && currentFrameIndex <= detectionEvent.FrameIndex + detectionEvent.DurationFrame)
                {
                    Collider[] colliders = SkillAttackDetectionTool.ShapeDetection(modelTransform, detectionEvent.AttackDetectionData, attackDetectionType, attackDetectionLayer);
                    if (colliders == null) break;
                    for (int c = 0; c < colliders.Length; c++)
                    {
                        if (colliders[c] != null)
                        {
                            onWeaponDetection?.Invoke(colliders[c]);
                        }
                    }
                }
            }

#if UNITY_EDITOR
            if (drawAttackDetectionGizmos)
            {
                if (currentFrameIndex >= detectionEvent.FrameIndex && currentFrameIndex <= detectionEvent.FrameIndex + detectionEvent.DurationFrame)
                {
                    currentAttackDetectionList.Add(detectionEvent);
                }
            }
#endif
        }
    }

    private IEnumerator AutoDestructEffectGameObject(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        obj.GameObjectPushPool();
    }



    #region Editor
#if UNITY_EDITOR
    [SerializeField] private bool drawAttackDetectionGizmos;
    private List<SkillAttackDetectionEvent> currentAttackDetectionList = new List<SkillAttackDetectionEvent>();

    private void OnDrawGizmos()
    {
        if (drawAttackDetectionGizmos && currentAttackDetectionList.Count != 0)
        {
            for (int i = 0; i < currentAttackDetectionList.Count; i++)
            {
                SkillGizmosTool.DrawDetection(currentAttackDetectionList[i], this);
            }
        }
    }
#endif
    #endregion
}
