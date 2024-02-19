using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������ࣺ���𱳰����򷽷�������ɾ�����ӷ���...
/// 
/// Info:�趨�ı����������Ϊ��
///     ����޷����ӳ��еĵ��ߣ�ֻ�ܶ�Cell��ʾ������Cell��NumΪ1
///     ���Ե��ӵĳ��е��ߣ�����������������������޷�ʰȡ
/// </summary>
public static class PackageUtil
{
    // TODO:���ߺ���

    /// <summary>
    /// �����������Ʒ
    /// </summary>
    /// <param name="propStats"></param>
    /// <returns>�Ƿ�ʰȡ�ɹ�</returns>
    public static bool AddProp(PropStats propStats)
    {
        List<PropStats> propList;
        bool isContainsSame = false; // �������Ƿ������ͬ�Ķ���
        if (DataManager.PlayerDataStats.PackageStatsData.PropDic.Dictionary.TryGetValue(propStats.PropType, out propList)) // ����ͬ����
        {
            if (!propStats.Overlayable) // ���ܵ��ӳ���
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
                if (propStats.Name == item.Name) // �Ƿ������ͬ�������ҿ��Ե���
                {
                    if (item.Num == item.Maximum) return false; // ������������
                    item.Num = Mathf.Clamp(item.Num + 1, 0, item.Maximum);
                    isContainsSame = true;
                }
            }
            if (!isContainsSame) // ��������ͬ����
            {
                propList.Add(propStats);
            }


        }
        else // ������ͬ����
        {
            DataManager.PlayerDataStats.PackageStatsData.PropDic.Dictionary.Add(propStats.PropType, new List<PropStats> { propStats });
        }

        DataManager.SavePackageData();
        return true;
    }

    /// <summary>
    /// �������Ƴ���Ʒ
    /// </summary>
    /// <param name="propStats">����</param>
    /// <param name="num">�Ƴ�����</param>
    public static void RemoveProp(PropStats propStats, int removeNum)
    {
        List<PropStats> propList;
        bool isContainsSame = false; // �������Ƿ������ͬ�Ķ���
        if (DataManager.PlayerDataStats.PackageStatsData.PropDic.Dictionary.TryGetValue(propStats.PropType, out propList)) // ����ͬ����
        {
            foreach (PropStats item in propList)
            {
                if (propStats.Name == item.Name) // �Ƿ������ͬ����
                {
                    if (removeNum > item.Num)
                    {
                        UISystem.AddTips("��������");
                    }
                    else
                    {
                        item.Num = Mathf.Clamp(item.Num - removeNum, 0, item.Maximum); // ����������0-99
                        isContainsSame = true;
                        if (item.Num == 0) // û�����Ƴ�
                        {
                            propList.Remove(item);
                        }
                    }

                }
            }
            // TODO:��һ��if�߼��Ƿ�ɾ����
            if (!isContainsSame)
            {
                Debug.Log("��ͬ���ͣ�û���������!");
            }
        }
        else // ������ͬ����
        {
            Debug.Log("û��ͬ���ͣ�û���������!");
        }

        DataManager.SavePackageData(); // �浵
    }

    /// <summary>
    /// ���ݵ������ͻ�ȡ��Ӧ�ĵ����б�
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
