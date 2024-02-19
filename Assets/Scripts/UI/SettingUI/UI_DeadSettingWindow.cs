using JKFrame;

[UIWindowData(nameof(UI_DeadSettingWindow), false, nameof(UI_DeadSettingWindow), 3)]
public class UI_DeadSettingWindow : UI_WindowBase
{
    public override void OnShow()
    {
        GameManager.Instance.SetCursorVisible(true);
    }
}
