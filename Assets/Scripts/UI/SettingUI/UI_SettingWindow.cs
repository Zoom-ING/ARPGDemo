using JKFrame;
using UnityEngine.UI;

/// <summary>
/// …Ë÷√¥∞ø⁄
/// </summary>
[UIWindowData(nameof(UI_SettingWindow), false, nameof(UI_SettingWindow), 1)]
public class UI_SettingWindow : UI_WindowBase
{
    public Slider GlobalAudioSlider;
    public Slider BGAudioSlider;
    public Slider EffectAudioSlider;
    public Button AudioResetButton;
    public Button GoBackMenuSceneButton;
    public Button QuitGameButton;

    public override void Init()
    {
        GlobalAudioSlider.value = AudioSystem.GlobalVolume;
        BGAudioSlider.value = AudioSystem.BGVolume;
        EffectAudioSlider.value = AudioSystem.EffectVolume;
        GlobalAudioSlider.onValueChanged.AddListener(GlobalAudioSliderValueChanged);
        BGAudioSlider.onValueChanged.AddListener(BGAudioSliderValueChanged);
        EffectAudioSlider.onValueChanged.AddListener(EffectAudioSliderValueChanged);
        AudioResetButton.onClick.AddListener(AudioResetButtonOnClicked);
    }

    private void GlobalAudioSliderValueChanged(float value)
    {
        AudioSystem.GlobalVolume = value;
    }

    private void BGAudioSliderValueChanged(float value)
    {
        AudioSystem.BGVolume = value;
    }

    private void EffectAudioSliderValueChanged(float value)
    {
        AudioSystem.EffectVolume = value;
    }

    private void AudioResetButtonOnClicked()
    {
        GlobalAudioSliderValueChanged(.5f);
        BGAudioSliderValueChanged(.5f);
        EffectAudioSliderValueChanged(.5f);
        GlobalAudioSlider.value = AudioSystem.GlobalVolume;
        BGAudioSlider.value = AudioSystem.BGVolume;
        EffectAudioSlider.value = AudioSystem.EffectVolume;
    }
}
