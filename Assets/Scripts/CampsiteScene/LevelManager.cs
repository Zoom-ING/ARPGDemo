using JKFrame;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMono<LevelManager>
{
    [LabelText("�ؿ�ͳ��")] public List<LevelConfig> levelStats = new List<LevelConfig>();
}
