using Sirenix.OdinInspector;
using System;

/// <summary>
/// 技能统计类
/// </summary>
[Serializable]
public class SkillStatsData
{
    /// <summary>
    /// 技能编号，技能数据
    /// </summary>
    [LabelText("获得的技能")] public Serialized_Dic<int, SkillData> AcquiredSkillDataDic;
    [LabelText("配备的技能")] public Serialized_Dic<int, SkillData> EquipedSkillDataDic;

    public void Init()
    {
        AcquiredSkillDataDic = new Serialized_Dic<int, SkillData>();
        EquipedSkillDataDic = new Serialized_Dic<int, SkillData>();
    }
}

[Serializable]
public class SkillData
{
    [LabelText("技能按键")] public string SkillKey;
    [LabelText("技能配置名称")] public string SkillConfigName;
    [LabelText("技能等级")] public int SkillLevel;

    public SkillData() { }
    public SkillData(string SkillKey, string SkillConfigName)
    {
        this.SkillKey = SkillKey;
        this.SkillConfigName = SkillConfigName;
    }
}

public enum SkillType
{
    Base, // 普攻
    CloseSkill, // 近身技能
    DistanceSkill // 远程技能
}
