using Sirenix.OdinInspector;
using System;

[Serializable]
public class PlayerDataStats
{
    [LabelText("���ְҵ")] public ProfessionType PlayerProfession;
    [LabelText("�Զ��岿λ���")] public CustomCharacterData CustomCharacterData;
    [LabelText("��ɫ����")] public CharacterData_ToSave CharacterData_ToSave;
    [LabelText("��������")] public SkillStatsData SkillStatsData; //TODO: ����Ӫ�غ�洢������ְҵ������playerController�п������ı䶯��ÿ�α䶯������һ�θ���浵��
    [LabelText("��������")] public PackageStatsData PackageStatsData;

    public void Init()
    {
        PlayerProfession = new ProfessionType();
        CustomCharacterData = new CustomCharacterData();
        SkillStatsData = new SkillStatsData();
        SkillStatsData.Init();
        PackageStatsData = new PackageStatsData();
        PackageStatsData.Init();
    }
}
