using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    [LabelText("World Canvas")] public Transform Canvas;

    [LabelText("Ѫ��Ԥ�Ƽ�����")] public string healthBarName;

    [LabelText("Ѫ�����ɵ�")] public Transform barPoint;

    [LabelText("�Ƿ�һֱ��ʾ")] public bool alwaysVisible;

    [LabelText("��ʾʱ��")] public float visibleTime;
    private float timeLeft; // ʣ����ʾʱ��

    private Image healthSlider;
    private GameObject UIBar;
    private Transform cam;

    private CharacterStats currentStats;

    private void Awake()
    {
        if (gameObject.tag == "Enemy")
        {
            currentStats = GetComponent<CharacterStats>();

            currentStats.UpdateHealthBarOnAttack += UpdateHealthBar; // ��ί���ڹ���ʱ����
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
        // Ѫ������
        if (UIBar != null)
        {
            UIBar.transform.position = barPoint.position;
            UIBar.transform.forward = -cam.forward; // Ѫ�����������

            if (timeLeft <= 0 && !alwaysVisible)
                UIBar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
        }
    }
}
