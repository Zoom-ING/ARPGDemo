using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包工具类：负责背包排序方法，道具删除增加方法...
/// 
/// Info:设定的背包容量情况为：
///     如果无法叠加持有的道具，只能多Cell显示，单个Cell的Num为1
///     可以叠加的持有道具，如果超过了最大的容量，将无法拾取
/// </summary>
public static class PackageUtil
{
    // TODO:工具函数

    /// <summary>
    /// 背包中添加物品
    /// </summary>
    /// <param name="propStats"></param>
    /// <returns>是否拾取成功</returns>
    public static bool AddProp(PropStats propStats)
    {
        List<PropStats> propList;
        bool isContainsSame = false; // 背包中是否包含相同的东西
        if (DataManager.PlayerDataStats.PackageStatsData.PropDic.Dictionary.TryGetValue(propStats.PropType, out propList)) // 包含同类型
        {
            if (!propStats.Overlayable) // 不能叠加持有
            {
                for (int i = 0; i < propStats.Num; i++)
                {
                    propList.Add(propStats);
                }

                DataManager.SavePackageData();
                return true;
            }

            foreach (PropStats item in propList)
            {
                if (propStats.Name == item.Name) // 是否包含相同东西，且可以叠加
                {
                    if (item.Num == item.Maximum) return false; // 超出持有上限
                    item.Num = Mathf.Clamp(item.Num + 1, 0, item.Maximum);
                    isContainsSame = true;
                }
            }
            if (!isContainsSame) // 不包含相同东西
            {
                propList.Add(propStats);
            }


        }
        else // 不包含同类型
        {
            DataManager.PlayerDataStats.PackageStatsData.PropDic.Dictionary.Add(propStats.PropType, new List<PropStats> { propStats });
        }

        DataManager.SavePackageData();
        return true;
    }

    /// <summary>
    /// 背包中移除物品
    /// </summary>
    /// <param name="propStats">道具</param>
    /// <param name="num">移除数量</param>
    public static void RemoveProp(PropStats propStats, int removeNum)
    {
        List<PropStats> propList;
        bool isContainsSame = false; // 背包中是否包含相同的东西
        if (DataManager.PlayerDataStats.PackageStatsData.PropDic.Dictionary.TryGetValue(propStats.PropType, out propList)) // 包含同类型
        {
            foreach (PropStats item in propList)
            {
                if (propStats.Name == item.Name) // 是否包含相同东西
                {
                    if (removeNum > item.Num)
                    {
                        UISystem.AddTips("数量不足");
                    }
                    else
                    {
                        item.Num = Mathf.Clamp(item.Num - removeNum, 0, item.Maximum); // 数量限制在0-99
                        isContainsSame = true;
                        if (item.Num == 0) // 没有了移除
                        {
                            propList.Remove(item);
                        }
                    }

                }
            }
            // TODO:这一段if逻辑是否删除？
            if (!isContainsSame)
            {
                Debug.Log("有同类型，没有这个东西!");
            }
        }
        else // 不包含同类型
        {
            Debug.Log("没有同类型，没有这个东西!");
        }

        DataManager.SavePackageData(); // 存档
    }

    /// <summary>
    /// 根据道具类型获取对应的道具列表
    /// </summary>
    /// <param name="propType"></param>
    /// <returns></returns>
    public static List<PropStats> GetPropStatsByPropType(PropType propType)
    {
        List<PropStats> propList;
        if (DataManager.PlayerDataStats.PackageStatsData.PropDic.Dictionary.TryGetValue(propType, out propList))
            return propList;
        else
            return null;
    }
}
