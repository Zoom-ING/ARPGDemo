using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͷ������
/// </summary>
[CreateAssetMenu(fileName = "ClothConfig_", menuName = "Config/CharacterPart/ClothConfig")]
public class ClothConfig : CharacterPartConfigBase
{
    [LabelText("��ɫIndex")] public int ColorIndex1;
    [LabelText("����ɫIndex")] public int ColorIndex2;
}
