using Sirenix.OdinInspector;
using System;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
public enum PropType
{
    [InspectorName("������ҩˮ")] BottleBuff,
    [InspectorName("װ��")] Equip,
    [InspectorName("ͨ�õ���")] UniversalProp,
}

/// <summary>
/// ����ͳ�ƣ����������ߣ��������...
/// </summary>
[Serializable]
public class PropStats
{
    [Header("��̬����")]

    [LabelText("����")] public PropType PropType;

    [LabelText("ID")] public int PropID;

    [LabelText("����")] public string Name;

    [LabelText("����")] public string Description;

    [LabelText("����")] public string Function;

    [LabelText("ʵ��Ԥ��������")] public string InstanceName;

    [LabelText("��ͼԤ�Ƽ�·��")] public string SpritePrefabPath;

    [LabelText("��������")] public int Maximum;

    [LabelText("�Ƿ���Ե��ӳ���")] public bool Overlayable;

    [Header("��̬����")]

    [LabelText("��������")] public int Num;

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
