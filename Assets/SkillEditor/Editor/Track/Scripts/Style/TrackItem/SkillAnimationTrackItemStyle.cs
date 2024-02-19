using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillAnimationTrackItemStyle: SkillTrackItemStyleBase
{
    private const string trackItemAssetPath = "Assets/SkillEditor/Editor/Track/Assets/TrackItem/AnimationTrackItem.uxml";
    private Label titleLabel;
    public VisualElement mainDragArea { get; private set; }
    public VisualElement animationOverLine { get; private set; }
    public void Init(SkillTrackStyleBase tracStyle, int startFrameIndex, float frameUnitWidth)
    {
        titleLabel = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackItemAssetPath).Instantiate().Query<Label>();
        root = titleLabel;
        mainDragArea = root.Q<VisualElement>("Main");
        animationOverLine = root.Q<VisualElement>("OverLine");
        tracStyle.AddItem(root);
    }

    public void SetTitle(string title)
    {
        titleLabel.text = title;
    }
}


