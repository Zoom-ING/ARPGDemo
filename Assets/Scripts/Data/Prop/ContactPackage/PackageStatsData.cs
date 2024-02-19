using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

/// <summary>
/// 背包数据统计类
/// </summary>
[Serializable]
public class PackageStatsData
{
    /// <summary>
    /// key:道具类型
    /// value:道具数据
    /// ex:增益道具：血瓶、魔力瓶...
    /// </summary>
    [LabelText("背包内所有道具的统计")] public Serialized_Dic<PropType, List<PropStats>> PropDic;

    public void Init()
    {
        PropDic = new Serialized_Dic<PropType, List<PropStats>>();
    }
}
