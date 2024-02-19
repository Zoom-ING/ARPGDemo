using JKFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DeadState : PlayerStateBase
{
    private UI_DeadFaderWindow deadFaderWindow;
    private UI_DeadSettingWindow deadSettingWindow;
    //private float fadeTimer;

    public override void Enter()
    {
        // Debug.Log("À¿Õˆ");
        deadFaderWindow = UISystem.Show<UI_DeadFaderWindow>();
        deadFaderWindow.Init();
        Player_Controller.Instance.StopAnimation();
        Player_Controller.Instance.PlayAnimation("Dead");
    }

    public override void Update()
    {
        if (deadFaderWindow.fader.alpha < 1f)
        {
            deadFaderWindow.fader.alpha += Time.deltaTime / 4f;
            if (deadFaderWindow.fader.alpha >= 1f)
            {
                deadSettingWindow = UISystem.Show<UI_DeadSettingWindow>();
                Time.timeScale = 0;
            }
        }
    }
}
