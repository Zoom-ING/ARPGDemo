﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillEffectTrackItemStyle : SkillTrackItemStyleBase
{
    private const string trackItemAssetPath = "Assets/SkillEditor/Editor/Track/Assets/TrackItem/AudioTrackItem.uxml";
    private Label titleLabel;
    public VisualElement mainDragArea { get; private set; }
    public bool isInit { get; private set; }
    public void Init(float frameUnitWidth, SkillEffectEvent skillEffectEvent, SkillMultilineTrackStyle.ChildTrack childTrack)
    {
        if (!isInit && skillEffectEvent.Prefab != null)
        {
            titleLabel = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackItemAssetPath).Instantiate().Query<Label>();
            root = titleLabel;
            mainDragArea = root.Q<VisualElement>("Main");
            childTrack.InitContent(root);
            isInit = true;
        }
    }

    public void ResetView(float frameUnitWidth, SkillEffectEvent skillEffectEvent)
    {
        if (!isInit) return;
        if (skillEffectEvent.Prefab != null)
        {
            SetTitle(skillEffectEvent.Prefab.name);
            SetWidth(frameUnitWidth * skillEffectEvent.Duration);
            SetPosition(frameUnitWidth * skillEffectEvent.FrameIndex);
        }
        else
        {
            SetTitle("");
            SetWidth(0);
            SetPosition(0);
        }
    }

    public void SetTitle(string title)
    {
        titleLabel.text = title;
    }
}
