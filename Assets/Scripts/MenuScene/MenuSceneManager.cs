using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;

public class MenuSceneManager : SingletonMono<MenuSceneManager>
{
    void Start()
    {
        UISystem.Show<UI_MenuSceneMenuWindow>();
    }
}
