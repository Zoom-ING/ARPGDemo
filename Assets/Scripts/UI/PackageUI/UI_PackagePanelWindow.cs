using JKFrame;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背包窗口
/// </summary>
[UIWindowData(nameof(UI_PackagePanelWindow), false, nameof(UI_PackagePanelWindow), 1)]
public class UI_PackagePanelWindow : UI_WindowBase
{
    [LabelText("背包内容显示区域")] public Transform PackageContentTfm;

    private int initSlotNum = 80;

    /// <summary>
    /// 当前展示的道具类型
    /// </summary>
    private PropType currentShowPropType = PropType.BottleBuff; // TODO:以后根据按钮的选项决定是哪种类型

    private List<UI_PackageCell> cellList = new List<UI_PackageCell>(); // 显示出来的Cell列表
    private List<UI_PackageCell> slotList = new List<UI_PackageCell>(); // 道具槽

    // TODO:在这将槽位初始化，里边的CellList不动
    //public override void Init()
    //{
    //    InitSlot();
    //    ShowPropCellByType(currentShowPropType);
    //}

    public override void OnShow()
    {
        // TODO:应该刷新而非初始化
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
    /// 根据道具类型显示道具UI
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
    /// 显示单个PropCell
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
    /// 刷新道具，用于背包窗口打开情况下捡起了道具
    /// </summary>
    public void UpdateProp(PropStats propStats)
    {
        if (propStats.PropType != currentShowPropType) return; // 同类型才会刷新

        //ClearCellList();

        //ShowPropCellByType(propStats.PropType);

        UpdatePropCellOnShow();
    }

    /// <summary>
    /// 打开背包时候实时更新
    /// </summary>
    private void UpdatePropCellOnShow()
    {
        if (cellList.Count == 0) // 背包没有就全部显示
        {
            ShowPropCellByType(currentShowPropType);
            return;
        }

        for (int j = cellList.Count - 1; j >= 0; j--) // 0个移除
        {
            if (cellList[j].Item.propStats.Num == 0) cellList[j].UnInit();
        }

        List<string> currStats = new List<string>(); // 当前背包的props的Name集合
        for (int i = 0; i < cellList.Count; i++)
        {
            currStats.Add(cellList[i].Item.propStats.Name);
        }

        List<PropStats> propStats = PackageUtil.GetPropStatsByPropType(currentShowPropType);
        for (int i = propStats.Count - 1; i >= 0; i--)
        {
            if (!currStats.Contains(propStats[i].Name) && propStats[i].Overlayable || !propStats[i].Overlayable) // 没有相同可叠加 || 不可叠加
            {
                ShowSingleCell(propStats[i]);
                propStats.RemoveAt(i);
                continue;
            }
            if (currStats.Contains(propStats[i].Name) && propStats[i].Overlayable) // 有相同可叠加
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
    /// 外部委托调用，添加使用中的Cell
    /// </summary>
    public void AddCellFromCellList(UI_PackageCell cell)
    {
        if (cell != null)
            cellList.Add(cell);
    }

    /// <summary>
    /// 外部委托调用，移除不使用的Cell
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
    /// 清理Cell列表
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
