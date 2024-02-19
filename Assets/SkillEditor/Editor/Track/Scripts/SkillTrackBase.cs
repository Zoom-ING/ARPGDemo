﻿using System;
using UnityEngine.UIElements;

public abstract class SkillTrackBase
{
    protected float frameWidth;

    public virtual void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
    {
        this.frameWidth = frameWidth;
    }

    public virtual void ResetView()
    {
        ResetView(frameWidth);
    }

    public virtual void ResetView(float frameWdith)
    {
        this.frameWidth = frameWdith;
    }

    public virtual void DeleteTrackItem(int frameIndex) { }
    public virtual void OnConfigChanged() { }
    public virtual void OnPlay(int startFrameIndex) { }
    public virtual void TickView(int frameIndex) { }
    public virtual void OnStop() { }
    public virtual void Destory() { }
    public virtual void DrawGizmos() { }

    public virtual void OnSceneGUI() { }
}
