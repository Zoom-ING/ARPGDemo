﻿using UnityEngine;

public class SkillEffectEvent
{
#if UNITY_EDITOR
    public string TrackName = "特效轨道";
#endif
    public int FrameIndex = -1;
    public GameObject Prefab;
    public Vector3 Position;
    public Vector3 Rotation;
    public Vector3 Scale;
    public int Duration;
    public bool AutoDestruct;
}