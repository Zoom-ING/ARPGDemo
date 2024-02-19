﻿using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioTrackItem : TrackItemBase<AudioTrack>
{
    private SkillMultilineTrackStyle.ChildTrack childTrackStyle;
    private SkillAudioTrackItemStyle trackItemStyle;

    private SkillAudioEvent skillAudioEvent;
    public SkillAudioEvent SkillAudioEvent { get => skillAudioEvent; }
    public void Init(AudioTrack track, float frameUnitWidth, SkillAudioEvent skillAudioEvent, SkillMultilineTrackStyle.ChildTrack childTrack)
    {
        this.track = track;
        this.frameIndex = skillAudioEvent.FrameIndex;
        this.childTrackStyle = childTrack;
        this.skillAudioEvent = skillAudioEvent;
        normalColor = new Color(0.388f, 0.850f, 0.905f, 0.5f);
        selectColor = new Color(0.388f, 0.850f, 0.905f, 1f);
        trackItemStyle = new SkillAudioTrackItemStyle();
        itemStyle = trackItemStyle;

        childTrackStyle.trackRoot.RegisterCallback<DragUpdatedEvent>(OnDragUpdate);
        childTrackStyle.trackRoot.RegisterCallback<DragExitedEvent>(DragExited);
        ResetView(frameUnitWidth);
    }

    public override void ResetView(float frameUnitWidth)
    {
        base.ResetView(frameUnitWidth);
        if (skillAudioEvent.AudioClip != null)
        {
            if (!trackItemStyle.isInit)
            {
                trackItemStyle.Init(frameUnitWidth, skillAudioEvent, childTrackStyle);
                // 绑定事件
                trackItemStyle.mainDragArea.RegisterCallback<MouseDownEvent>(MouseDown);
                trackItemStyle.mainDragArea.RegisterCallback<MouseUpEvent>(MouseUp);
                trackItemStyle.mainDragArea.RegisterCallback<MouseOutEvent>(MouseOut);
                trackItemStyle.mainDragArea.RegisterCallback<MouseMoveEvent>(MouseMove);
            }
        }
        trackItemStyle.ResetView(frameUnitWidth, skillAudioEvent);
    }

    public void Destroy()
    {
        childTrackStyle.Destroy();
    }

    public void SetTrackName(string name)
    {
        childTrackStyle.SetTrackName(name);
    }

    #region 鼠标交互
    private bool mouseDrag = false;
    private float startDargPosX;
    private int startDragFrameIndex;
    private void MouseDown(MouseDownEvent evt)
    {
        startDargPosX = evt.mousePosition.x;
        startDragFrameIndex = frameIndex;
        mouseDrag = true;
        Select();
    }
    private void MouseUp(MouseUpEvent evt)
    {
        if (mouseDrag) ApplyDrag();
        mouseDrag = false;
    }

    private void MouseOut(MouseOutEvent evt)
    {
        if (mouseDrag) ApplyDrag();
        mouseDrag = false;
    }

    private void MouseMove(MouseMoveEvent evt)
    {
        if (mouseDrag)
        {
            float offsetPos = evt.mousePosition.x - startDargPosX;
            int offsetFrame = Mathf.RoundToInt(offsetPos / frameUnitWidth);
            int targetFrameIndex = startDragFrameIndex + offsetFrame;

            // 不考虑拖拽到负数的情况 以及 没有便宜
            if (targetFrameIndex < 0 || offsetFrame == 0) return;

            // 确定修改的数据
            frameIndex = targetFrameIndex;
            skillAudioEvent.FrameIndex = frameIndex;
            // 如果超过右侧边界，拓展边界
            // CheckFrameCount();
            // 刷新视图
            ResetView(frameUnitWidth);
        }
    }

    public void CheckFrameCount()
    {
        int frameCount = (int)(skillAudioEvent.AudioClip.length * SkillEditorWindow.Instance.SkillConfig.FrameRote);
        // 如果超过右侧边界，拓展边界
        if (frameIndex + frameCount > SkillEditorWindow.Instance.SkillConfig.FrameCount)
        {
            // 保存配置导致对象无效，重新引用
            SkillEditorWindow.Instance.CurrentFrameCount = frameIndex + frameCount;
        }
    }

    private void ApplyDrag()
    {
        if (startDragFrameIndex != frameIndex)
        {
            skillAudioEvent.FrameIndex = frameIndex;
            SkillEditorInspector.Instance.SetTrackItemFrameIndex(frameIndex);
        }
    }
    #endregion

    #region 拖拽资源
    private void OnDragUpdate(DragUpdatedEvent evt)
    {
        // 监听用户拖拽的是否是动画
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        AudioClip clip = objs[0] as AudioClip;
        if (clip != null)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
    }
    private void DragExited(DragExitedEvent evt)
    {
        // 监听用户拖拽的是否是动画
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        AudioClip clip = objs[0] as AudioClip;
        if (clip != null)
        {
            int selectFrameIndex = SkillEditorWindow.Instance.GetFrameIndexByPos(evt.localMousePosition.x);
            if (selectFrameIndex >= 0)
            {
                // 构建默认的音效数据
                skillAudioEvent.AudioClip = clip;
                skillAudioEvent.FrameIndex = selectFrameIndex;
                skillAudioEvent.Voluem = 1;
                this.frameIndex = selectFrameIndex;
                ResetView();
                SkillEditorWindow.Instance.SaveConfig();
            }
        }
    }
    #endregion
}
