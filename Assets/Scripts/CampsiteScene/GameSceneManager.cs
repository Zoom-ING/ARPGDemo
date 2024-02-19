using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
public class GameSceneManager : SingletonMono<GameSceneManager>
{
    #region 测试逻辑
    public bool IsTest;
    public bool IsCreateArchive;
    #endregion
    private void Start()
    {
        #region 测试逻辑
        if (IsTest)
        {
            if (IsCreateArchive)
            {
                DataManager.CreateArchive();
            }
            else
            {
                DataManager.LoadCurrentArchive();
            }
        }
        #endregion
        // 初始化角色
        Player_Controller.Instance.Init();

        UISystem.Show<UI_PlayerStateBar>();

        GameManager.Instance.SetCursorVisible(false);
    }
}
