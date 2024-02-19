using JKFrame;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������
/// </summary>
[UIWindowData(nameof(UI_PackagePanelWindow), false, nameof(UI_PackagePanelWindow), 1)]
public class UI_PackagePanelWindow : UI_WindowBase
{
    [LabelText("����������ʾ����")] public Transform PackageContentTfm;

    private int initSlotNum = 80;

    /// <summary>
    /// ��ǰչʾ�ĵ�������
    /// </summary>
    private PropType currentShowPropType = PropType.BottleBuff; // TODO:�Ժ���ݰ�ť��ѡ���������������

    private List<UI_PackageCell> cellList = new List<UI_PackageCell>(); // ��ʾ������Cell�б�
    private List<UI_PackageCell> slotList = new List<UI_PackageCell>(); // ���߲�

    // TODO:���⽫��λ��ʼ������ߵ�CellList����
    //public override void Init()
    //{
    //    InitSlot();
    //    ShowPropCellByType(currentShowPropType);
    //}

    public override void OnShow()
    {
        // TODO:Ӧ��ˢ�¶��ǳ�ʼ��
        InitSlot();
        ShowPropCellByType(currentShowPropType);
    }

    private void InitSlot()
    {
        for (int i = 0; i < initSlotNum; i++)
        {
            UI_PackageCell slot = ResSystem.InstantiateGameObject<UI_PackageCell>("UI_PackageCell", PackageContentTfm);
            slotList.Add(slot);
        }
    }

    /// <summary>
    /// ���ݵ���������ʾ����UI
    /// </summary>
    private void ShowPropCellByType(PropType propType)
    {
        ClearCellList();

        List<PropStats> propStats = PackageUtil.GetPropStatsByPropType(PropType.BottleBuff);

        if (propStats != null)
        {
            for (int i = 0; i < propStats.Count; i++)
            {
                slotList[i].Init(propStats[i]);
            }
        }
    }

    /// <summary>
    /// ��ʾ����PropCell
    /// </summary>
    /// <param name="propStats"></param>
    private void ShowSingleCell(PropStats propStats)
    {
        if (propStats != null)
        {
            foreach (var slot in slotList)
            {
                if (slot.showCell != true)
                {
                    slot.Init(propStats);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// ˢ�µ��ߣ����ڱ������ڴ�����¼����˵���
    /// </summary>
    public void UpdateProp(PropStats propStats)
    {
        if (propStats.PropType != currentShowPropType) return; // ͬ���ͲŻ�ˢ��

        //ClearCellList();

        //ShowPropCellByType(propStats.PropType);

        UpdatePropCellOnShow();
    }

    /// <summary>
    /// �򿪱���ʱ��ʵʱ����
    /// </summary>
    private void UpdatePropCellOnShow()
    {
        if (cellList.Count == 0) // ����û�о�ȫ����ʾ
        {
            ShowPropCellByType(currentShowPropType);
            return;
        }

        for (int j = cellList.Count - 1; j >= 0; j--) // 0���Ƴ�
        {
            if (cellList[j].Item.propStats.Num == 0) cellList[j].UnInit();
        }

        List<string> currStats = new List<string>(); // ��ǰ������props��Name����
        for (int i = 0; i < cellList.Count; i++)
        {
            currStats.Add(cellList[i].Item.propStats.Name);
        }

        List<PropStats> propStats = PackageUtil.GetPropStatsByPropType(currentShowPropType);
        for (int i = propStats.Count - 1; i >= 0; i--)
        {
            if (!currStats.Contains(propStats[i].Name) && propStats[i].Overlayable || !propStats[i].Overlayable) // û����ͬ�ɵ��� || ���ɵ���
            {
                ShowSingleCell(propStats[i]);
                propStats.RemoveAt(i);
                continue;
            }
            if (currStats.Contains(propStats[i].Name) && propStats[i].Overlayable) // ����ͬ�ɵ���
            {
                for (int j = 0; j < cellList.Count; j++)
                {
                    if (cellList[j].Item.propStats.Name == propStats[i].Name)
                    {
                        cellList[j].UpdateCell(propStats[i]);
                        propStats.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// �ⲿί�е��ã����ʹ���е�Cell
    /// </summary>
    public void AddCellFromCellList(UI_PackageCell cell)
    {
        if (cell != null)
            cellList.Add(cell);
    }

    /// <summary>
    /// �ⲿί�е��ã��Ƴ���ʹ�õ�Cell
    /// </summary>
    public void RemoveCellFromCellList(UI_PackageCell cell)
    {
        cellList.Remove(cell);
    }

    public override void OnClose()
    {
        //ClearCellList();
    }

    /// <summary>
    /// ����Cell�б�
    /// </summary>
    private void ClearCellList()
    {
        if (cellList.Count != 0)
        {
            for (int i = cellList.Count - 1; i >= 0; i--)
            {
                cellList[i].UnInit();
                //cellList[i].gameObject.GameObjectPushPool();
            }

            cellList.Clear();
        }
    }

    private void ClearSlotList()
    {
        if (slotList != null)
        {
            for (int i = slotList.Count - 1; i >= 0; i--)
            {
                slotList[i].UnInit();
                cellList[i].GameObjectPushPool();
            }

            slotList.Clear();
        }
    }
}
