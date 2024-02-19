using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Íæ¼Ò×´Ì¬Ìõ
/// </summary>
[UIWindowData(nameof(UI_PlayerStateBar), false, nameof(UI_PlayerStateBar), 0)]
public class UI_PlayerStateBar : UI_WindowBase
{
    private Image healthSlider; // ÑªÌõ

    private void Awake()
    {
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
    }

    private void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        float sliderPercent = (float)Player_Controller.Instance.stats.characterData.currentHealth / Player_Controller.Instance.stats.characterData.maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
}
