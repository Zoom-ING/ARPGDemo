using JKFrame;
using UnityEngine;
/// <summary>
/// 数据管理器
/// </summary>
public static class DataManager
{
    static DataManager()
    {
        LoadSaveData();
    }

    public static PlayerDataStats PlayerDataStats;

    /// <summary>
    /// 是否有存档
    /// </summary>
    public static bool HaveArchive { get; private set; }

    private static void LoadSaveData()
    {
        SaveItem saveItem = SaveSystem.GetSaveItem(0);
        HaveArchive = saveItem != null;
    }

    public static void SaveData()
    {
        SaveSystem.SaveObject(PlayerDataStats);
    }

    /// <summary>
    /// 创建新存档
    /// </summary>
    public static void CreateArchive()
    {
        if (HaveArchive)
        {
            // 删除全部存档
            SaveSystem.DeleteAllSaveItem();
        }

        // 创建一个存档
        SaveSystem.CreateSaveItem();

        PlayerDataStats = new PlayerDataStats();
        PlayerDataStats.Init();

        // 初始化角色外观数据
        InitCustomCharacterData();
        // 初始化技能数据
        InitPlayerSkillData();

        SaveSkillStatsData();
        SaveCustomCharacterData();
    }

    /// <summary>
    /// 加载当前存档
    /// </summary>
    public static void LoadCurrentArchive()
    {
        PlayerDataStats = SaveSystem.LoadObject<PlayerDataStats>();
        CustomCharacterData = PlayerDataStats.CustomCharacterData;
        GameManager.Instance.PlayerProfession = PlayerDataStats.PlayerProfession;
        //LoadCharacterStats();

        // TODO：加载存档的技能数据
        // 技能数据“无形”存储，直接修改PlayerDataStats.SkillStatsData里的两个技能词典
    }

    #region 玩家数据
    public static CustomCharacterData CustomCharacterData;

    public static void InitCustomCharacterData()
    {
        CustomCharacterData = new CustomCharacterData();
        CustomCharacterData.CustomPartDataDic = new Serialized_Dic<int, CustomCharacterPartData>();
        CustomCharacterData.CustomPartDataDic.Dictionary.Add((int)CharacterPartType.Face, new CustomCharacterPartData()
        {
            Index = 1,
            Size = 1,
            Height = 0
        });
        CustomCharacterData.CustomPartDataDic.Dictionary.Add((int)CharacterPartType.Hair, new CustomCharacterPartData()
        {
            Index = 1,
            Color1 = Color.white.ConverToSerializationColor()
        }); ;
        CustomCharacterData.CustomPartDataDic.Dictionary.Add((int)CharacterPartType.Cloth, new CustomCharacterPartData()
        {
            Index = 1,
            Color1 = Color.white.ConverToSerializationColor(),
            Color2 = Color.black.ConverToSerializationColor()
        });
    }

    public static void SavePlayerProfessionData()
    {
        PlayerDataStats.PlayerProfession = GameManager.Instance.PlayerProfession;
        SaveSystem.SaveObject(PlayerDataStats);
    }

    public static void SaveCustomCharacterData()
    {
        PlayerDataStats.CustomCharacterData = CustomCharacterData;
        SaveSystem.SaveObject(PlayerDataStats);
    }
    #endregion

    #region 技能数据

    public static void InitPlayerSkillData()
    {
        // 习得技能表
        PlayerDataStats.SkillStatsData.AcquiredSkillDataDic.Dictionary.Add(0, new SkillData("MouseAttack1", "WarriorBaseAttack1Config"));
        PlayerDataStats.SkillStatsData.AcquiredSkillDataDic.Dictionary.Add(1, new SkillData("MouseAttack2", "WarriorBaseAttack2Config"));
        PlayerDataStats.SkillStatsData.AcquiredSkillDataDic.Dictionary.Add(2, new SkillData("MouseAttack3", "WarriorBaseAttack3Config"));

        // 配备技能表
        PlayerDataStats.SkillStatsData.EquipedSkillDataDic.Dictionary.Add(0, new SkillData("MouseAttack1", "WarriorBaseAttack1Config"));
        PlayerDataStats.SkillStatsData.EquipedSkillDataDic.Dictionary.Add(1, new SkillData("MouseAttack2", "WarriorBaseAttack2Config"));
        PlayerDataStats.SkillStatsData.EquipedSkillDataDic.Dictionary.Add(2, new SkillData("MouseAttack3", "WarriorBaseAttack3Config"));
    }

    public static void SaveSkillStatsData()
    {
        SaveSystem.SaveObject(PlayerDataStats);
    }
    #endregion

    #region 角色数据

    //TODO:还没有测试角色数据的存档功能
    public static void SaveCharacterStats()
    {
        PlayerDataStats.CharacterData_ToSave.maxHealth = Player_Controller.Instance.stats.characterData.maxHealth;
        PlayerDataStats.CharacterData_ToSave.currentHealth = Player_Controller.Instance.stats.characterData.currentHealth;
        PlayerDataStats.CharacterData_ToSave.baseDefense = Player_Controller.Instance.stats.characterData.baseDefense;
        PlayerDataStats.CharacterData_ToSave.currentDefense = Player_Controller.Instance.stats.characterData.currentDefense;

        PlayerDataStats.CharacterData_ToSave.minAttack = Player_Controller.Instance.stats.characterData.minAttack;
        PlayerDataStats.CharacterData_ToSave.maxAttack = Player_Controller.Instance.stats.characterData.maxAttack;
        PlayerDataStats.CharacterData_ToSave.criticalChance = Player_Controller.Instance.stats.characterData.criticalChance;
        PlayerDataStats.CharacterData_ToSave.criticalMultiple = Player_Controller.Instance.stats.characterData.criticalMultiple;
        PlayerDataStats.CharacterData_ToSave.baseAttackCoolDown = Player_Controller.Instance.stats.characterData.baseAttackCoolDown;
        PlayerDataStats.CharacterData_ToSave.baseAttackRange = Player_Controller.Instance.stats.characterData.baseAttackRange;
        PlayerDataStats.CharacterData_ToSave.holdSkills = Player_Controller.Instance.stats.characterData.holdSkills;

        PlayerDataStats.CharacterData_ToSave.killExp = Player_Controller.Instance.stats.characterData.killExp;

        PlayerDataStats.CharacterData_ToSave.currentLevel = Player_Controller.Instance.stats.characterData.currentLevel;
        PlayerDataStats.CharacterData_ToSave.maxLevel = Player_Controller.Instance.stats.characterData.maxLevel;
        PlayerDataStats.CharacterData_ToSave.baseExp = Player_Controller.Instance.stats.characterData.baseExp;
        PlayerDataStats.CharacterData_ToSave.currentExp = Player_Controller.Instance.stats.characterData.currentExp;
        PlayerDataStats.CharacterData_ToSave.levelBuff = Player_Controller.Instance.stats.characterData.levelBuff;

        SaveSystem.SaveObject(PlayerDataStats);
    }

    /// <summary>
    /// 加载角色数据，在PlayerController的Init中调用？本类中加载存档调用？
    /// </summary>
    public static void LoadCharacterStats()
    {
        Player_Controller.Instance.stats.characterData.maxHealth = PlayerDataStats.CharacterData_ToSave.maxHealth;
        Player_Controller.Instance.stats.characterData.currentHealth = PlayerDataStats.CharacterData_ToSave.currentHealth;
        Player_Controller.Instance.stats.characterData.baseDefense = PlayerDataStats.CharacterData_ToSave.baseDefense;
        Player_Controller.Instance.stats.characterData.currentDefense = PlayerDataStats.CharacterData_ToSave.currentDefense;

        Player_Controller.Instance.stats.characterData.minAttack = PlayerDataStats.CharacterData_ToSave.minAttack;
        Player_Controller.Instance.stats.characterData.maxAttack = PlayerDataStats.CharacterData_ToSave.maxAttack;
        Player_Controller.Instance.stats.characterData.criticalChance = PlayerDataStats.CharacterData_ToSave.criticalChance;
        Player_Controller.Instance.stats.characterData.criticalMultiple = PlayerDataStats.CharacterData_ToSave.criticalMultiple;
        Player_Controller.Instance.stats.characterData.baseAttackCoolDown = PlayerDataStats.CharacterData_ToSave.baseAttackCoolDown;
        Player_Controller.Instance.stats.characterData.baseAttackRange = PlayerDataStats.CharacterData_ToSave.baseAttackRange;
        Player_Controller.Instance.stats.characterData.holdSkills = PlayerDataStats.CharacterData_ToSave.holdSkills;

        Player_Controller.Instance.stats.characterData.killExp = PlayerDataStats.CharacterData_ToSave.killExp;

        Player_Controller.Instance.stats.characterData.currentLevel = PlayerDataStats.CharacterData_ToSave.currentLevel;
        Player_Controller.Instance.stats.characterData.maxLevel = PlayerDataStats.CharacterData_ToSave.maxLevel;
        Player_Controller.Instance.stats.characterData.baseExp = PlayerDataStats.CharacterData_ToSave.baseExp;
        Player_Controller.Instance.stats.characterData.currentExp = PlayerDataStats.CharacterData_ToSave.currentExp;
        Player_Controller.Instance.stats.characterData.levelBuff = PlayerDataStats.CharacterData_ToSave.levelBuff;
    }

    #endregion

    #region 背包数据
    /// <summary>
    /// 保存背包数据
    /// </summary>
    public static void SavePackageData()
    {
        //SaveSystem.SaveObject(PlayerDataStats.PackageStatsData); TODO:以后单独保存，最后关闭游戏全局存档
        SaveSystem.SaveObject(PlayerDataStats);
    }
    #endregion
}
