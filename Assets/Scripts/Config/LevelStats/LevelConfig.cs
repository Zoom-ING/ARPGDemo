using JKFrame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ؿ���Ϣͳ�ƣ�����Ӫ�س���ѡ��ؿ�����
/// </summary>
[CreateAssetMenu(menuName = "Config/LevelConfig", fileName = "LevelConfig")]
public class LevelConfig : ConfigBase
{
    [LabelText("�ؿ�����")] public string LevelName;
    [LabelText("�ؿ���������")] public string LevelSceneName;
    [LabelText("�ؿ��Ѷ�")] public LevelDifficulty LevelDifficultyType;
}

public enum LevelDifficulty
{
    Easy,
    Medium,
    Difficulty,
    Hell
}
