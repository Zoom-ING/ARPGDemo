using JKFrame;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    public ProfessionType PlayerProfession;

    UI_LoadProcessWindow loadUIWindow;

    /// <summary>
    /// 创建新存档，并且进入游戏
    /// </summary>
    public void CreateNewArchiveAndEnterGame()
    {
        // 初始化存档
        DataManager.CreateArchive();
        // 进入自定义角色场景
        loadUIWindow = UISystem.Show<UI_LoadProcessWindow>();
        SceneSystem.LoadSceneAsync("CreateCharacter", LoadProgress);
        EventSystem.AddEventListener("LoadSceneSucceed", LoadSceneSucceed);
    }

    /// <summary>
    /// 使用旧存档，进入游戏
    /// </summary>
    public void UseCurrentArchiveAndEnterGame()
    {
        // 加载当前存档
        DataManager.LoadCurrentArchive();
        // 进入游戏场景
        loadUIWindow = UISystem.Show<UI_LoadProcessWindow>();
        SceneSystem.LoadSceneAsync("Campsite", LoadProgress);
        EventSystem.AddEventListener("LoadSceneSucceed", LoadSceneSucceed);
    }

    void LoadProgress(float progress)
    {
        loadUIWindow.PeocessText.text = $"{progress * 100}%";
        loadUIWindow.ProcessBarImage.fillAmount = progress;
    }
    void LoadSceneSucceed()
    {
        UISystem.Close<UI_LoadProcessWindow>();
        loadUIWindow = null;
    }

    /// <summary>
    /// 光标显隐
    /// </summary>
    public void SetCursorVisible(bool whetherVisible)
    {
        Cursor.visible = whetherVisible;
    }
}
