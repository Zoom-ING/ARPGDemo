using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using Sirenix.OdinInspector;
/// <summary>
/// ��Ŀ����
/// </summary>
[CreateAssetMenu(fileName = "ProjectConfig",menuName = "Config/ProjectConfig")]
public class ProjectConfig : ConfigBase
{
    #region �Զ����ɫ����
    [BoxGroup("�Զ����ɫ����"),LabelText("�Զ���ɫ������ID")]
    public Dictionary<CharacterPartType, List<int>> CustomCharacterPartConfigIDDic;
    #endregion
}
