using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

/// <summary>
/// ��������ͳ����
/// </summary>
[Serializable]
public class PackageStatsData
{
    /// <summary>
    /// key:��������
    /// value:��������
    /// ex:������ߣ�Ѫƿ��ħ��ƿ...
    /// </summary>
    [LabelText("���������е��ߵ�ͳ��")] public Serialized_Dic<PropType, List<PropStats>> PropDic;

    public void Init()
    {
        PropDic = new Serialized_Dic<PropType, List<PropStats>>();
    }
}
