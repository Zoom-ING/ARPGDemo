using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(SkillEditorWindow))]
public class SkillEditorInspector : Editor
{
    public static SkillEditorInspector Instance;
    public static TrackItemBase currentTrackItem { get; private set; }
    private static SkillTrackBase currentTrack;
    public static void SetTrackItem(TrackItemBase trackItem, SkillTrackBase track)
    {
        if (currentTrackItem != null)
        {
            currentTrackItem.OnUnSelect();
        }
        currentTrackItem = trackItem;
        currentTrackItem.OnSelect();
        currentTrack = track;
        // 避免已经打开了Inspector，不刷新数据
        if (Instance != null) Instance.Show();

    }
    private void OnDestroy()
    {
        // 说明窗口卸载
        if (currentTrackItem != null)
        {
            currentTrackItem.OnUnSelect();
            currentTrackItem = null;
            currentTrack = null;
        }
    }
    private VisualElement root;
    public override VisualElement CreateInspectorGUI()
    {
        Instance = this;
        root = new VisualElement();
        Show();
        return root;
    }

    private void Show()
    {
        Clean();
        if (currentTrackItem == null) return;

        // TODO:补充其他类型
        Type itemType = currentTrackItem.GetType();
        if (itemType == typeof(AnimationTrackItem))
        {
            DrawAniamtionTrackItem((AnimationTrackItem)currentTrackItem);
        }
        else if (itemType == typeof(AudioTrackItem))
        {
            DrawAudioTrackItem((AudioTrackItem)currentTrackItem);
        }
        else if (itemType == typeof(EffectTrackItem))
        {
            DrawEffectTrackItem((EffectTrackItem)currentTrackItem);
        }
        else if (itemType == typeof(AttackDetectionTrackItem))
        {
            DrawAttackDetectionTrackItem((AttackDetectionTrackItem)currentTrackItem);
        }
    }

    private void Clean()
    {
        if (root != null)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                root.RemoveAt(i);
            }
        }
    }
    private int trackItemFrameIndex;
    public void SetTrackItemFrameIndex(int trackItemFrameIndex)
    {
        this.trackItemFrameIndex = trackItemFrameIndex;
    }

    #region 动画轨道
    private Label clipFrameLabel;
    private Toggle rootMotionToggle;
    private Label isLoopLable;
    private IntegerField durationField;
    private FloatField transitionTimeField;

    private void DrawAniamtionTrackItem(AnimationTrackItem animationTrackItem)
    {
        trackItemFrameIndex = animationTrackItem.FrameIndex;
        // 动画资源
        ObjectField animationClipAssetField = new ObjectField("动画资源");
        animationClipAssetField.objectType = typeof(AnimationClip);
        animationClipAssetField.value = animationTrackItem.AnimationEvent.AnimationClip;
        animationClipAssetField.RegisterValueChangedCallback(AnimationClipAssetFiedlValueChanged);
        root.Add(animationClipAssetField);

        // 根运动
        rootMotionToggle = new Toggle("应用根运动");
        rootMotionToggle.value = animationTrackItem.AnimationEvent.ApplyRootMotion;
        rootMotionToggle.RegisterValueChangedCallback(RootMotionToggleValueChanged);
        root.Add(rootMotionToggle);

        // 轨道长度
        durationField = new IntegerField("轨道长度");
        durationField.value = animationTrackItem.AnimationEvent.DurationFrame;
        durationField.RegisterCallback<FocusInEvent>(DurationFieldFocusIn);
        durationField.RegisterCallback<FocusOutEvent>(DurationFieldFocusOut);
        root.Add(durationField);

        // 过渡时间
        transitionTimeField = new FloatField("过渡时间");
        transitionTimeField.value = animationTrackItem.AnimationEvent.TransitionTime;
        transitionTimeField.RegisterCallback<FocusInEvent>(TransitionTimeFieldFocusIn);
        transitionTimeField.RegisterCallback<FocusOutEvent>(TransitionTimeFieldFocusOut);
        root.Add(transitionTimeField);

        // 动画相关的信息
        int clipFrameCount = (int)(animationTrackItem.AnimationEvent.AnimationClip.length * animationTrackItem.AnimationEvent.AnimationClip.frameRate);
        clipFrameLabel = new Label("动画资源长度:" + clipFrameCount);
        root.Add(clipFrameLabel);
        isLoopLable = new Label("循环动画:" + animationTrackItem.AnimationEvent.AnimationClip.isLooping);
        root.Add(isLoopLable);

        // 删除
        Button deleteButton = new Button(DeleteButtonClick);
        deleteButton.text = "删除";
        deleteButton.style.backgroundColor = new Color(1, 0, 0, 0.5f);
        root.Add(deleteButton);

        // 设置持续帧数至选中帧
        Button setFrameButton = new Button(SetAnimationDurationFrameButton);
        setFrameButton.text = "设置持续帧数至选中帧";
        root.Add(setFrameButton);
    }

    private void AnimationClipAssetFiedlValueChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        AnimationClip clip = evt.newValue as AnimationClip;
        // 修改自身显示效果
        clipFrameLabel.text = "动画资源长度:" + ((int)(clip.length * clip.frameRate));
        isLoopLable.text = "循环动画:" + clip.isLooping;
        // 保存到配置
        ((AnimationTrackItem)currentTrackItem).AnimationEvent.AnimationClip = clip;
        SkillEditorWindow.Instance.SaveConfig();
        currentTrackItem.ResetView();
    }

    private void RootMotionToggleValueChanged(ChangeEvent<bool> evt)
    {
        ((AnimationTrackItem)currentTrackItem).AnimationEvent.ApplyRootMotion = evt.newValue;
        SkillEditorWindow.Instance.SaveConfig();
    }

    int oldDurationValue;
    private void DurationFieldFocusIn(FocusInEvent evt)
    {
        oldDurationValue = durationField.value;
    }
    private void DurationFieldFocusOut(FocusOutEvent evt)
    {
        if (durationField.value != oldDurationValue)
        {
            // 安全校验
            if (((AnimationTrack)currentTrack).CheckFrameIndexOnDrag(trackItemFrameIndex + durationField.value, trackItemFrameIndex, false))
            {
                // 修改数据，刷新视图
                ((AnimationTrackItem)currentTrackItem).AnimationEvent.DurationFrame = durationField.value;
                ((AnimationTrackItem)currentTrackItem).CheckFrameCount();
                SkillEditorWindow.Instance.SaveConfig();
                currentTrackItem.ResetView();
            }
            else
            {
                durationField.value = oldDurationValue;
            }
        }
    }
    private void SetAnimationDurationFrameButton()
    {
        DurationFieldFocusIn(null);
        durationField.value = SkillEditorWindow.Instance.CurrentSelectFrameIndex - ((AnimationTrackItem)currentTrackItem).FrameIndex;
        DurationFieldFocusOut(null);
    }

    float oldTransitionTimeValue;
    private void TransitionTimeFieldFocusIn(FocusInEvent evt)
    {
        oldTransitionTimeValue = transitionTimeField.value;
    }
    private void TransitionTimeFieldFocusOut(FocusOutEvent evt)
    {
        if (transitionTimeField.value != oldTransitionTimeValue)
        {
            ((AnimationTrackItem)currentTrackItem).AnimationEvent.TransitionTime = transitionTimeField.value;
        }
    }

    private void DeleteButtonClick()
    {
        currentTrack.DeleteTrackItem(trackItemFrameIndex); // 此函数提供保存和刷新视图逻辑
        Selection.activeObject = null;
    }
    #endregion

    #region 音效轨道
    private FloatField voluemFiled;
    private void DrawAudioTrackItem(AudioTrackItem trackItem)
    {
        // 动画资源
        ObjectField audioClipAssetField = new ObjectField("音效资源");
        audioClipAssetField.objectType = typeof(AudioClip);
        audioClipAssetField.value = trackItem.SkillAudioEvent.AudioClip;
        audioClipAssetField.RegisterValueChangedCallback(AudioClipAssetFiedlValueChanged);
        root.Add(audioClipAssetField);

        // 音量
        voluemFiled = new FloatField("播放音量");
        voluemFiled.value = trackItem.SkillAudioEvent.Voluem;
        voluemFiled.RegisterCallback<FocusInEvent>(VoluemFiledFocusIn);
        voluemFiled.RegisterCallback<FocusOutEvent>(VoluemFiledFocusOut);
        root.Add(voluemFiled);
    }

    private void AudioClipAssetFiedlValueChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        AudioClip audioClip = evt.newValue as AudioClip;
        // 保存到配置中
        ((AudioTrackItem)currentTrackItem).SkillAudioEvent.AudioClip = audioClip;
        currentTrackItem.ResetView();
    }

    float oldVoluemFiledValue;
    private void VoluemFiledFocusIn(FocusInEvent evt)
    {
        oldVoluemFiledValue = voluemFiled.value;
    }
    private void VoluemFiledFocusOut(FocusOutEvent evt)
    {
        if (voluemFiled.value != oldVoluemFiledValue)
        {
            ((AudioTrackItem)currentTrackItem).SkillAudioEvent.Voluem = voluemFiled.value;
        }
    }
    #endregion

    #region 特效轨道

    private IntegerField effectDurationFiled;
    private void DrawEffectTrackItem(EffectTrackItem trackItem)
    {
        // 预制体
        ObjectField effectPrefabAssetField = new ObjectField("特效预制体");
        effectPrefabAssetField.objectType = typeof(GameObject);
        effectPrefabAssetField.value = trackItem.SkillEffectEvent.Prefab;
        effectPrefabAssetField.RegisterValueChangedCallback(EffectPrefabAssetFiedlValueChanged);
        root.Add(effectPrefabAssetField);

        // 坐标
        Vector3Field posFiled = new Vector3Field("坐标");
        posFiled.value = trackItem.SkillEffectEvent.Position;
        posFiled.RegisterValueChangedCallback(EffectPosFiledValueChanged);
        root.Add(posFiled);

        // 旋转
        Vector3Field rotFiled = new Vector3Field("旋转");
        rotFiled.value = trackItem.SkillEffectEvent.Rotation;
        rotFiled.RegisterValueChangedCallback(EffectRotFiledValueChanged);
        root.Add(rotFiled);

        // 旋转
        Vector3Field scaleFiled = new Vector3Field("缩放");
        scaleFiled.value = trackItem.SkillEffectEvent.Scale;
        scaleFiled.RegisterValueChangedCallback(EffectScaleFiledValueChanged);
        root.Add(scaleFiled);

        // 自动销毁
        Toggle autoDestructToggle = new Toggle("自动销毁");
        autoDestructToggle.value = trackItem.SkillEffectEvent.AutoDestruct;
        autoDestructToggle.RegisterValueChangedCallback(EffectAutoDestructToggleValueChanged);
        root.Add(autoDestructToggle);

        // 时间
        effectDurationFiled = new IntegerField("持续时间");
        effectDurationFiled.value = trackItem.SkillEffectEvent.Duration;
        effectDurationFiled.RegisterCallback<FocusInEvent>(EffectDurationFiledFocusIn);
        effectDurationFiled.RegisterCallback<FocusOutEvent>(EffectDurationFiledFocusOut);
        root.Add(effectDurationFiled);

        // 时间计算按钮
        Button calculateEffectButton = new Button(CalculateEffectDuration);
        calculateEffectButton.text = "重新计时";
        root.Add(calculateEffectButton);

        // 引用模型Transform属性
        Button applyModelTransformDataButton = new Button(ApplyModelTransformData);
        applyModelTransformDataButton.text = "引用模型Transform属性";
        root.Add(applyModelTransformDataButton);

        // 设置持续帧数至选中帧
        Button setFrameButton = new Button(SetEffectDurationFrameButton);
        setFrameButton.text = "设置持续帧数至选中帧";
        root.Add(setFrameButton);

    }

    private void ApplyModelTransformData()
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        effectTrackItem.ApplyModelTransformData();
        Show();
    }

    private void CalculateEffectDuration()
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        ParticleSystem[] particleSystems = effectTrackItem.SkillEffectEvent.Prefab.GetComponentsInChildren<ParticleSystem>();

        float max = -1;
        int curr = -1;
        for (int i = 0; i < particleSystems.Length; i++)
        {
            if (particleSystems[i].main.duration > max)
            {
                max = particleSystems[i].main.duration;
                curr = i;
            }
        }

        effectTrackItem.SkillEffectEvent.Duration = (int)(particleSystems[curr].main.duration * SkillEditorWindow.Instance.SkillConfig.FrameRote);
        effectDurationFiled.value = effectTrackItem.SkillEffectEvent.Duration;
        effectTrackItem.ResetView();
    }

    private void EffectPrefabAssetFiedlValueChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        effectTrackItem.SkillEffectEvent.Prefab = evt.newValue as GameObject;
        // 重新计时
        CalculateEffectDuration();
        effectTrackItem.ResetView();
    }

    private void EffectPosFiledValueChanged(ChangeEvent<Vector3> evt)
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        effectTrackItem.SkillEffectEvent.Position = evt.newValue;
        effectTrackItem.ResetView();
    }

    private void EffectRotFiledValueChanged(ChangeEvent<Vector3> evt)
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        effectTrackItem.SkillEffectEvent.Rotation = evt.newValue;
        effectTrackItem.ResetView();
    }

    private void EffectScaleFiledValueChanged(ChangeEvent<Vector3> evt)
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        effectTrackItem.SkillEffectEvent.Scale = evt.newValue;
        effectTrackItem.ResetView();
    }

    private void EffectAutoDestructToggleValueChanged(ChangeEvent<bool> evt)
    {
        EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
        effectTrackItem.SkillEffectEvent.AutoDestruct = evt.newValue;
    }

    float oldEffectDurationFiled;
    private void EffectDurationFiledFocusIn(FocusInEvent evt)
    {
        oldEffectDurationFiled = effectDurationFiled.value;
    }
    private void EffectDurationFiledFocusOut(FocusOutEvent evt)
    {
        if (effectDurationFiled.value != oldEffectDurationFiled)
        {
            EffectTrackItem effectTrackItem = ((EffectTrackItem)currentTrackItem);
            effectTrackItem.SkillEffectEvent.Duration = effectDurationFiled.value;
            effectTrackItem.ResetView();
        }
    }

    private void SetEffectDurationFrameButton()
    {
        EffectDurationFiledFocusIn(null);
        effectDurationFiled.value = SkillEditorWindow.Instance.CurrentSelectFrameIndex - ((EffectTrackItem)currentTrackItem).FrameIndex;
        EffectDurationFiledFocusOut(null);
    }

    #endregion

    #region 攻击检测轨道
    private IntegerField detectionDurationFrameField;
    private List<string> detectionChoiceList;
    private void DrawAttackDetectionTrackItem(AttackDetectionTrackItem trackItem)
    {
        detectionDurationFrameField = new IntegerField("持续帧数");
        detectionDurationFrameField.value = trackItem.SkillAttackDetectionEvent.DurationFrame;
        detectionDurationFrameField.RegisterValueChangedCallback(DurationFrameFieldValueChanged);
        root.Add(detectionDurationFrameField);

        detectionChoiceList = new List<string>(Enum.GetNames(typeof(AttackDetectionType)));
        DropdownField detectionDropDownField = new DropdownField("检测类型", detectionChoiceList, (int)trackItem.SkillAttackDetectionEvent.AttackDetectionType);
        detectionDropDownField.RegisterValueChangedCallback(OnDetectionDropDownFieldValueChanged);
        root.Add(detectionDropDownField);
        // 根据检测类型进行绘制
        switch (trackItem.SkillAttackDetectionEvent.AttackDetectionType)
        {
            case AttackDetectionType.Weapon:
                AttackWeaponDetectionData weaponDetectionData = (AttackWeaponDetectionData)trackItem.SkillAttackDetectionEvent.AttackDetectionData;
                DropdownField weaponDetectionDropDownField = new DropdownField("武器选择");
                if (SkillEditorWindow.Instance.PreviewCharacterObj != null)
                {
                    Skill_Player skill_Player = SkillEditorWindow.Instance.PreviewCharacterObj.GetComponent<Skill_Player>();
                    weaponDetectionDropDownField.choices = skill_Player.WeaponDic.Keys.ToList();
                }

                if (!string.IsNullOrEmpty(weaponDetectionData.weaponName))
                {
                    weaponDetectionDropDownField.value = weaponDetectionData.weaponName;
                }
                weaponDetectionDropDownField.RegisterValueChangedCallback(WeaponDetectionDropDownFieldValueChanged);
                root.Add(weaponDetectionDropDownField);
                break;
            case AttackDetectionType.Box:
                AttackBoxDetectionData boxDetectionData = (AttackBoxDetectionData)trackItem.SkillAttackDetectionEvent.AttackDetectionData;
                Vector3Field boxDetectionPositionField = new Vector3Field("坐标");
                Vector3Field boxDetectionRotationField = new Vector3Field("旋转");
                Vector3Field boxDetectionScaleField = new Vector3Field("缩放");
                boxDetectionPositionField.value = boxDetectionData.Position;
                boxDetectionRotationField.value = boxDetectionData.Rotation;
                boxDetectionScaleField.value = boxDetectionData.Scale;
                boxDetectionPositionField.RegisterValueChangedCallback(ShapeDetectionPositionFieldValueChanged);
                boxDetectionRotationField.RegisterValueChangedCallback(BoxDetectionRotationFieldValueChanged);
                boxDetectionScaleField.RegisterValueChangedCallback(BoxDetectionScaleFieldValueChanged);
                root.Add(boxDetectionPositionField);
                root.Add(boxDetectionRotationField);
                root.Add(boxDetectionScaleField);
                break;
            case AttackDetectionType.Sphere:
                AttackSphereDetectionData sphereDetectionData = (AttackSphereDetectionData)trackItem.SkillAttackDetectionEvent.AttackDetectionData;
                Vector3Field sphereDetectionPositionField = new Vector3Field("坐标");
                sphereDetectionPositionField.RegisterValueChangedCallback(ShapeDetectionPositionFieldValueChanged);

                FloatField sphereRadiusField = new FloatField("半径");
                sphereRadiusField.value = sphereDetectionData.Radius;
                sphereRadiusField.RegisterValueChangedCallback(SphereRadiusFieldValueChanged);

                root.Add(sphereDetectionPositionField);
                root.Add(sphereRadiusField);
                break;
            case AttackDetectionType.Fan:
                AttackFanDetectionData fanDetectionData = (AttackFanDetectionData)trackItem.SkillAttackDetectionEvent.AttackDetectionData;

                Vector3Field fanDetectionPositionField = new Vector3Field("坐标");
                Vector3Field fanDetectionRotationField = new Vector3Field("旋转");
                FloatField fanInsideRadiusFiled = new FloatField("内半径");
                FloatField fanRadiusField = new FloatField("外半径");
                FloatField fanHeightField = new FloatField("高度");
                FloatField fanAangle = new FloatField("角度");

                fanDetectionPositionField.value = fanDetectionData.Position;
                fanDetectionRotationField.value = fanDetectionData.Rotation;
                fanInsideRadiusFiled.value = fanDetectionData.InsideRadius;
                fanRadiusField.value = fanDetectionData.Radius;
                fanHeightField.value = fanDetectionData.Height;
                fanAangle.value = fanDetectionData.Angle;

                fanDetectionPositionField.RegisterValueChangedCallback(ShapeDetectionPositionFieldValueChanged);
                fanDetectionRotationField.RegisterValueChangedCallback(FanDetectionRotationFieldValueChanged);
                fanInsideRadiusFiled.RegisterValueChangedCallback(FanInsideRadiusFieldValueChanged);
                fanRadiusField.RegisterValueChangedCallback(FanRadiusFieldValueChanged);
                fanHeightField.RegisterValueChangedCallback(FanHeightFieldValueChanged);
                fanAangle.RegisterValueChangedCallback(FanAngleFieldValueChanged);
                root.Add(fanDetectionPositionField);
                root.Add(fanDetectionRotationField);
                root.Add(fanInsideRadiusFiled);
                root.Add(fanRadiusField);
                root.Add(fanHeightField);
                root.Add(fanAangle);
                break;
        }

        // 设置持续帧数至选中帧
        Button setFrameButton = new Button(SetDetectionDurationFrameButton);
        setFrameButton.text = "设置持续帧数至选中帧";
        root.Add(setFrameButton);
    }


    private void WeaponDetectionDropDownFieldValueChanged(ChangeEvent<string> evt)
    {
        AttackWeaponDetectionData detectionData = (AttackWeaponDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.weaponName = evt.newValue;
    }

    private void DurationFrameFieldValueChanged(ChangeEvent<int> evt)
    {
        AttackDetectionTrackItem attackDetectionTrackItem = (AttackDetectionTrackItem)currentTrackItem;
        attackDetectionTrackItem.SkillAttackDetectionEvent.DurationFrame = evt.newValue;
        currentTrackItem.ResetView();
    }

    private void SetDetectionDurationFrameButton()
    {
        detectionDurationFrameField.value = SkillEditorWindow.Instance.CurrentSelectFrameIndex - ((AttackDetectionTrackItem)currentTrackItem).FrameIndex;
    }


    private void OnDetectionDropDownFieldValueChanged(ChangeEvent<string> evt)
    {
        AttackDetectionTrackItem attackDetectionTrackItem = (AttackDetectionTrackItem)currentTrackItem;
        attackDetectionTrackItem.SkillAttackDetectionEvent.AttackDetectionType = (AttackDetectionType)detectionChoiceList.IndexOf(evt.newValue);
        Show();
    }
    private void ShapeDetectionPositionFieldValueChanged(ChangeEvent<Vector3> evt)
    {
        AttackShapeDetectionDataBase shapeDetectionData = (AttackShapeDetectionDataBase)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        shapeDetectionData.Position = evt.newValue;
    }
    private void BoxDetectionRotationFieldValueChanged(ChangeEvent<Vector3> evt)
    {
        AttackBoxDetectionData detectionData = (AttackBoxDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.Rotation = evt.newValue;
    }

    private void BoxDetectionScaleFieldValueChanged(ChangeEvent<Vector3> evt)
    {
        AttackBoxDetectionData detectionData = (AttackBoxDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.Scale = evt.newValue;
    }

    private void SphereRadiusFieldValueChanged(ChangeEvent<float> evt)
    {
        AttackSphereDetectionData detectionData = (AttackSphereDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.Radius = evt.newValue;
    }

    private void FanDetectionRotationFieldValueChanged(ChangeEvent<Vector3> evt)
    {
        AttackFanDetectionData detectionData = (AttackFanDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.Rotation = evt.newValue;
    }

    private void FanInsideRadiusFieldValueChanged(ChangeEvent<float> evt)
    {
        AttackFanDetectionData detectionData = (AttackFanDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.InsideRadius = evt.newValue;
        if (detectionData.Radius <= detectionData.InsideRadius)
        {
            detectionData.InsideRadius = detectionData.Radius - 0.01f;
            Show();
        }
    }

    private void FanRadiusFieldValueChanged(ChangeEvent<float> evt)
    {
        AttackFanDetectionData detectionData = (AttackFanDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.Radius = evt.newValue;
        if (detectionData.Radius <= detectionData.InsideRadius)
        {
            detectionData.InsideRadius = detectionData.Radius - 0.01f;
            Show();
        }
    }

    private void FanHeightFieldValueChanged(ChangeEvent<float> evt)
    {
        AttackFanDetectionData detectionData = (AttackFanDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.Height = evt.newValue;
        if (detectionData.Height <= 0)
        {
            detectionData.Height = 0.01f;
            Show();
        }
    }

    private void FanAngleFieldValueChanged(ChangeEvent<float> evt)
    {
        AttackFanDetectionData detectionData = (AttackFanDetectionData)((AttackDetectionTrackItem)currentTrackItem).SkillAttackDetectionEvent.AttackDetectionData;
        detectionData.Angle = evt.newValue;
        if (detectionData.Angle < 0)
        {
            detectionData.Angle = 0.1f;
            Show();
        }
        else if (detectionData.Angle > 360)
        {
            detectionData.Angle = 360;
            Show();
        }
    }

    #endregion
}
