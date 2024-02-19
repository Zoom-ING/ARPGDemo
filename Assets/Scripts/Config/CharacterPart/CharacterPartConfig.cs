using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using Sirenix.OdinInspector;

/// <summary>
/// ��ɫ�Ĳ�λ����
/// </summary>
public enum CharacterPartType
{ 
    [LabelText("����")] Face,
    [LabelText("ͷ��")] Hair,
    [LabelText("����")] Belt,
    [LabelText("����")] Cloth,
    [LabelText("ñ��")] Hat,
    [LabelText("����")] Glove,
    [LabelText("Ь��")] Shoe,
    [LabelText("�粿")] ShoulderPad,
}

/// <summary>
/// ��λ����
/// </summary>
public abstract class CharacterPartConfigBase:ConfigBase
{
    [LabelText("����")] public string Name;
    [LabelText("֧�ֵ�ְҵ")] public List<ProfessionType> ProfessionTypes;
    [LabelText("��λ")] public CharacterPartType CharacterPartType;
    [LabelText("������")] public Mesh Mesh1;
}
