using Sirenix.OdinInspector;
using System;

/// <summary>
/// ����ͳ����
/// </summary>
[Serializable]
public class SkillStatsData
{
    /// <summary>
    /// ���ܱ�ţ���������
    /// </summary>
    [LabelText("��õļ���")] public Serialized_Dic<int, SkillData> AcquiredSkillDataDic;
    [LabelText("�䱸�ļ���")] public Serialized_Dic<int, SkillData> EquipedSkillDataDic;

    public void Init()
    {
        AcquiredSkillDataDic = new Serialized_Dic<int, SkillData>();
        EquipedSkillDataDic = new Serialized_Dic<int, SkillData>();
    }
}

[Serializable]
public class SkillData
{
    [LabelText("���ܰ���")] public string SkillKey;
    [LabelText("������������")] public string SkillConfigName;
    [LabelText("���ܵȼ�")] public int SkillLevel;

    public SkillData() { }
    public SkillData(string SkillKey, string SkillConfigName)
    {
        this.SkillKey = SkillKey;
        this.SkillConfigName = SkillConfigName;
    }
}

public enum SkillType
{
    Base, // �չ�
    CloseSkill, // ������
    DistanceSkill // Զ�̼���
}
