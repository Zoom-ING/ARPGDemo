using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͷ������
/// </summary>
[CreateAssetMenu(fileName = "HairConfig_", menuName = "Config/CharacterPart/HairConfig")]
public class HairConfig : CharacterPartConfigBase
{
    /// <summary>
    /// ��ɫ����,-1����ζ����Ч
    /// </summary>
    [LabelText("��ɫIndex")] public int ColorIndex;
    [LabelText("��ͷ����")] public Mesh Mesh2;
}
