using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 道具的基类
/// </summary>
public class Prop_Base : MonoBehaviour
{
    [LabelText("数据预制件")] public PropDataBase DataPrefab;

    [LabelText("拾取音效")] public AudioClip PickUpAudio;
}
