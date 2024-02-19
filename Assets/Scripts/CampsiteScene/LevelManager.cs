using JKFrame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMono<LevelManager>
{
    [LabelText("¹Ø¿¨Í³¼Æ")] public List<LevelConfig> levelStats = new List<LevelConfig>();
}
