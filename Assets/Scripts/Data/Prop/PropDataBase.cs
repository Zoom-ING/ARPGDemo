using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 道具数据基类 SO
/// </summary>
public class PropDataBase : ScriptableObject
{
    [LabelText("道具信息")] public PropStats propStats;
}
