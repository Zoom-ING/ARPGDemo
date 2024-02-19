using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包物品的信息说明窗口
/// </summary>
public class UI_PropInfo : MonoBehaviour
{
    [LabelText("名字Text")] public Text PropName;
    [LabelText("描述Text")] public Text PropDescription;
    [LabelText("功能Text")] public Text PropFunc;

    public void Init(PropStats propStats)
    {
        PropName.text = propStats.Name;
        PropDescription.text = propStats.Description;
        PropFunc.text = propStats.Function;
    }
}
