using JKFrame;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[UIWindowData(nameof(UI_LevelPrepareWindow), false, nameof(UI_LevelPrepareWindow), 2)]
public class UI_LevelPrepareWindow : UI_WindowBase
{
    [SerializeField] Button quitButton;
    [SerializeField] Transform levelItemRoot;
    private List<UI_LevelItem> UI_LevelItemList = new List<UI_LevelItem>();

    public override void Init()
    {
        Player_Controller.Instance.CanControl = false;
        quitButton.onClick.AddListener(QuitButtonClick);
    }

    public override void OnShow()
    {
        GameManager.Instance.SetCursorVisible(true);
        base.OnShow();
        UpdateLevelItem();
    }

    private void UpdateLevelItem()
    {
        // 清空已有的
        for (int i = 0; i < UI_LevelItemList.Count; i++)
        {
            UI_LevelItemList[i].Destrot();
        }
        UI_LevelItemList.Clear();

        // 放置新的
        List<LevelConfig> levelItems = LevelManager.Instance.levelStats;
        for (int i = 0; i < levelItems.Count; i++)
        {
            UI_LevelItem item = ResSystem.InstantiateGameObject<UI_LevelItem>("LevelItemWindow");
            item.Init(levelItems[i]);
            item.transform.SetParent(levelItemRoot);
            UI_LevelItemList.Add(item);
        }
    }

    private void QuitButtonClick()
    {
        UISystem.Close<UI_LevelPrepareWindow>();
    }

    public override void OnClose()
    {
        GameManager.Instance.SetCursorVisible(false);
        Time.timeScale = 1.0f;
    }
}
