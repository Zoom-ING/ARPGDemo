using Cinemachine;
using JKFrame;
using UnityEngine;

/// <summary>
/// 窗口类型
/// </summary>
public enum MenuType
{
    None,
    Setting,
    Package,
}

/// <summary>
/// 菜单控制器，控制菜单的显示
/// </summary>
public class MenuController : MonoBehaviour
{
    public AudioClip ResponseAudio;

    private string packageWindowName = "UI_PackagePanelWindow";
    private string settingWindowName = "UI_SettingWindow";
    private MenuType currentMenuType = MenuType.None;

    public CinemachineFreeLook FreeLook;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenOrCloseMenu(packageWindowName, MenuType.Package);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenOrCloseMenu(settingWindowName, MenuType.Setting);
        }
    }

    private void OpenOrCloseMenu(string windowName, MenuType menuType)
    {
        if (currentMenuType == menuType)
        {
            AudioSystem.PlayOneShot(ResponseAudio, Player_Controller.Instance);
            FreeLook.enabled = true;
            UISystem.Close(windowName);
            Player_Controller.Instance.CanControl = true;
            GameManager.Instance.SetCursorVisible(false);
            currentMenuType = MenuType.None;
        }
        else if (currentMenuType == MenuType.None)
        {
            AudioSystem.PlayOneShot(ResponseAudio, Player_Controller.Instance);
            FreeLook.enabled = false;
            UISystem.Show(windowName).Init();
            Player_Controller.Instance.CanControl = false;
            GameManager.Instance.SetCursorVisible(true);
            currentMenuType = menuType;
        }
    }
}
