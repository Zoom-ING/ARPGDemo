using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 背包中的小血瓶数据
/// </summary>
[CreateAssetMenu(fileName = "New Data", menuName = "PropData/LittleBloodBuff")]
public class UIBloodBuffData_SO : PropDataBase
{
    [LabelText("增益百分比")] public float BloodBuff;
}
