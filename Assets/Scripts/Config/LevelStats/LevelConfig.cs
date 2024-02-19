using JKFrame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡信息统计，用于营地场景选择关卡界面
/// </summary>
[CreateAssetMenu(menuName = "Config/LevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ConfigBase
{
    [LabelText("关卡名称")] public string LevelName;
    [LabelText("关卡场景名称")] public string LevelSceneName;
    [LabelText("关卡难度")] public LevelDifficulty LevelDifficultyType;
}

public enum LevelDifficulty
{
    Easy,
    Medium,
    Difficulty,
    Hell
}
