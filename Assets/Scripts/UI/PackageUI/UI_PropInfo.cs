using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������Ʒ����Ϣ˵������
/// </summary>
public class UI_PropInfo : MonoBehaviour
{
    [LabelText("����Text")] public Text PropName;
    [LabelText("����Text")] public Text PropDescription;
    [LabelText("����Text")] public Text PropFunc;

    public void Init(PropStats propStats)
    {
        PropName.text = propStats.Name;
        PropDescription.text = propStats.Description;
        PropFunc.text = propStats.Function;
    }
}
