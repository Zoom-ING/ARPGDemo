using JKFrame;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ������Ʒ��
/// </summary>
public class UI_PackageCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CellItem Item;

    public Transform OnSuspend;

    [HideInInspector] public bool showCell = false; // �ò�λ�Ƿ���ʾ�˵���

    private Action<UI_PackageCell> onInit;
    private Action<UI_PackageCell> onUnInit;

    public AudioClip AcrossAudio;

    private void Awake()
    {
        onInit = UISystem.GetWindow<UI_PackagePanelWindow>().AddCellFromCellList;
        onUnInit = UISystem.GetWindow<UI_PackagePanelWindow>().RemoveCellFromCellList;
    }

    public void Init(PropStats propStats)
    {
        onInit?.Invoke(this);
        showCell = true;
        Item.gameObject.SetActive(true);
        Item.Init(propStats, this);
    }

    /// <summary>
    /// ����Item������ݣ�ͼƬ����Ŀ�ȣ���ɾ��
    /// </summary>
    public void UpdateCell(PropStats propStats)
    {
        Item.UpdateCell(propStats);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioSystem.PlayOneShot(AcrossAudio, Player_Controller.Instance, false, 0.01f, false);
        OnSuspend.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnSuspend.gameObject.SetActive(false);
    }

    public void UnInit()
    {
        onUnInit?.Invoke(this);
        OnSuspend.gameObject.SetActive(false);
        Item.gameObject.SetActive(false);
        showCell = false;
        onInit = null;
        onUnInit = null;
    }
}
