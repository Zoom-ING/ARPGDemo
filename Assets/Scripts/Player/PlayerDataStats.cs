using Sirenix.OdinInspector;
using System;

[Serializable]
public class PlayerDataStats
{
    [LabelText("玩家职业")] public ProfessionType PlayerProfession;
    [LabelText("自定义部位外观")] public CustomCharacterData CustomCharacterData;
    [LabelText("角色数据")] public CharacterData_ToSave CharacterData_ToSave;
    [LabelText("技能数据")] public SkillStatsData SkillStatsData; //TODO: 进入营地后存储？根据职业？；在playerController中控制它的变动，每次变动都保存一次该类存档？
    [LabelText("背包数据")] public PackageStatsData PackageStatsData;

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
