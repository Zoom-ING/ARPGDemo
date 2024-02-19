using Sirenix.OdinInspector;
using System;
using UnityEngine;

/// <summary>
/// 道具类型
/// </summary>
public enum PropType
{
    [InspectorName("增减益药水")] BottleBuff,
    [InspectorName("装备")] Equip,
    [InspectorName("通用道具")] UniversalProp,
}

/// <summary>
/// 道具统计：武器，防具，增益道具...
/// </summary>
[Serializable]
public class PropStats
{
    [Header("静态数据")]

    [LabelText("类型")] public PropType PropType;

    [LabelText("ID")] public int PropID;

    [LabelText("名称")] public string Name;

    [LabelText("描述")] public string Description;

    [LabelText("功能")] public string Function;

    [LabelText("实例预制体名称")] public string InstanceName;

    [LabelText("贴图预制件路径")] public string SpritePrefabPath;

    [LabelText("持有上限")] public int Maximum;

    [LabelText("是否可以叠加持有")] public bool Overlayable;

    [Header("动态数据")]

    [LabelText("持有数量")] public int Num;

    public PropStats(PropStats propStats)
    {
        this.PropType = propStats.PropType;
        this.PropID = propStats.PropID;
        this.Name = propStats.Name;
        this.Description = propStats.Description;
        this.Function = propStats.Function;
        this.InstanceName = propStats.InstanceName;
        this.SpritePrefabPath = propStats.SpritePrefabPath;
        this.Maximum = propStats.Maximum;
        this.Overlayable = propStats.Overlayable;
        this.Num = propStats.Num;
    }
}
