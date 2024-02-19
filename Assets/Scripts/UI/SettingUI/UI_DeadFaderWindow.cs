using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 死亡后设置的过渡窗口(深红背景)
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
        satireTextList.Add("菜");
        satireTextList.Add("凉");
        satireTextList.Add("要复活吗?");
        satireTextList.Add("看来你需要更多练习才能赢得胜利。");
        satireTextList.Add("死亡是失败的代名词，你可要小心了。");
        satireTextList.Add("看来你还有很长的路要走才能成为真正的大师。");
        satireTextList.Add("别泄气，失败是成功之母，继续努力吧！");
        satireTextList.Add("看来你需要重新检查你的战术了。");
        satireTextList.Add("这次的表现可真是糟糕透顶。");
        satireTextList.Add("你的对手可不会给你第二次机会。");
        satireTextList.Add("死亡并不可怕，但是连续死亡就有点可怕了。");
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
