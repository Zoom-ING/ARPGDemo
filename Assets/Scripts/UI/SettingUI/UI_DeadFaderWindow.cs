using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���������õĹ��ɴ���(��챳��)
/// </summary>
[UIWindowData(nameof(UI_DeadFaderWindow), false, nameof(UI_DeadFaderWindow), 3)]
public class UI_DeadFaderWindow : UI_WindowBase
{
    [HideInInspector] public CanvasGroup fader;
    private List<string> satireTextList = new List<string>();
    public Text SatireText;

    public override void Init()
    {
        fader = GetComponent<CanvasGroup>();
        satireTextList.Add("��");
        satireTextList.Add("��");
        satireTextList.Add("Ҫ������?");
        satireTextList.Add("��������Ҫ������ϰ����Ӯ��ʤ����");
        satireTextList.Add("������ʧ�ܵĴ����ʣ����ҪС���ˡ�");
        satireTextList.Add("�����㻹�кܳ���·Ҫ�߲��ܳ�Ϊ�����Ĵ�ʦ��");
        satireTextList.Add("��й����ʧ���ǳɹ�֮ĸ������Ŭ���ɣ�");
        satireTextList.Add("��������Ҫ���¼�����ս���ˡ�");
        satireTextList.Add("��εı��ֿ��������͸����");
        satireTextList.Add("��Ķ��ֿɲ������ڶ��λ��ᡣ");
        satireTextList.Add("�����������£����������������е�����ˡ�");
    }

    public override void OnShow()
    {
        int textIndex = Random.Range(0, satireTextList.Count);
        if (textIndex > 2) SatireText.fontSize = 60;
        SatireText.text = satireTextList[textIndex];
    }

    public override void OnClose()
    {
        base.OnClose();
    }

}
