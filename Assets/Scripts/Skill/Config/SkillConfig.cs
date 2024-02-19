using JKFrame;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/SkillConfig", fileName = "SkillConfig")]
public class SkillConfig : ConfigBase
{
    [Header("Base")]
    [LabelText("技能名称")] public string SkillName;
    [LabelText("技能类型")] public SkillType SkillType;
    [LabelText("伤害次数")] public int DamageCount;
    [LabelText("最小伤害")] public int MinDamage;
    [LabelText("最大伤害")] public int MaxDamage;
    [LabelText("帧数上限")] public int FrameCount = 100;
    [LabelText("帧率")] public int FrameRote = 30;
    [LabelText("是否持续伤害")] public bool SustainedDamage;

    [Header("持续伤害")] // 持续伤害

    [Header("近身攻击")]

    [Header("远程攻击")]

    [NonSerialized, OdinSerialize] public SkillAnimationData SkillAnimationData = new SkillAnimationData();
    [NonSerialized, OdinSerialize] public SkillAudioData SkillAudioData = new SkillAudioData();
    [NonSerialized, OdinSerialize] public SkillEffectData SkillEffectData = new SkillEffectData();
    [NonSerialized, OdinSerialize] public SkillAttackDetectionData SkillAttackDetectionData = new SkillAttackDetectionData();

#if UNITY_EDITOR
    private static Action skillConfigValidate;
    public static void SetValidateAction(Action action)
    {
        skillConfigValidate = action;
    }

    private void OnValidate()
    {
        skillConfigValidate?.Invoke();
    }
#endif
}
