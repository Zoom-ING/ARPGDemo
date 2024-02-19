using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    [LabelText("World Canvas")] public Transform Canvas;

    [LabelText("血条预制件名称")] public string healthBarName;

    [LabelText("血条生成点")] public Transform barPoint;

    [LabelText("是否一直显示")] public bool alwaysVisible;

    [LabelText("显示时间")] public float visibleTime;
    private float timeLeft; // 剩余显示时间

    private Image healthSlider;
    private GameObject UIBar;
    private Transform cam;

    private CharacterStats currentStats;

    private void Awake()
    {
        if (gameObject.tag == "Enemy")
        {
            currentStats = GetComponent<CharacterStats>();

            currentStats.UpdateHealthBarOnAttack += UpdateHealthBar; // 此委托在攻击时调用
        }
    }

    private void Start()
    {

        cam = Camera.main.transform;
        UIBar = ResSystem.InstantiateGameObject(healthBarName, Canvas);
        //UIBar = Instantiate(healthBarUIPrefab, canvas.transform).transform;
        healthSlider = UIBar.transform.GetChild(0).GetComponent<Image>();
        healthSlider.fillAmount = (float)currentStats.characterData.currentHealth / currentStats.characterData.maxHealth;
        UIBar.gameObject.SetActive(alwaysVisible);
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0 && UIBar.gameObject != null)
        {
            Destroy(UIBar.gameObject);
        }
        if (UIBar.gameObject == null) return;

        UIBar.gameObject.SetActive(true);
        timeLeft = visibleTime;

        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }

    private void LateUpdate()
    {
        // 血条跟随
        if (UIBar != null)
        {
            UIBar.transform.position = barPoint.position;
            UIBar.transform.forward = -cam.forward; // 血条面向摄像机

            if (timeLeft <= 0 && !alwaysVisible)
                UIBar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
        }
    }
}
