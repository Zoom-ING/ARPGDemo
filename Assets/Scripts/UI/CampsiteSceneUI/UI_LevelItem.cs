using JKFrame;
using UnityEngine;
using UnityEngine.UI;

[UIWindowData(nameof(UI_LevelItem), false, "LevelItemWindow", 2)]
public class UI_LevelItem : UI_WindowBase
{
    [SerializeField] private Text LevelNameText;
    [SerializeField] private Text DifficultyText;
    [SerializeField] private Button accessButton;
    private LevelConfig levelConfig;

    UI_LoadProcessWindow loadUIWindow;

    public void Init(LevelConfig levelConfig)
    {
        this.levelConfig = levelConfig;
        LevelNameText.text = levelConfig.LevelName;
        DifficultyText.text = levelConfig.LevelDifficultyType.ToString();
        accessButton.onClick.AddListener(AccessLevelScene);
    }

    private void AccessLevelScene()
    {
        UISystem.Close<UI_LevelPrepareWindow>();
        DestroyImmediate(Player_Controller.Instance.gameObject);
        loadUIWindow = UISystem.Show<UI_LoadProcessWindow>();
        SceneSystem.LoadSceneAsync(levelConfig.LevelSceneName, LoadProgress);
        EventSystem.AddEventListener("LoadSceneSucceed", LoadSceneSucceed);
    }

    void LoadProgress(float progress)
    {
        loadUIWindow.PeocessText.text = $"{progress * 100}%";
        loadUIWindow.ProcessBarImage.fillAmount = progress;
    }
    void LoadSceneSucceed()
    {
        UISystem.Close<UI_LoadProcessWindow>();// 警告：已经关闭了，仍关闭
        loadUIWindow = null;
    }

    public void Destrot()
    {
        levelConfig = null;
        this.GameObjectPushPool();
    }
}
