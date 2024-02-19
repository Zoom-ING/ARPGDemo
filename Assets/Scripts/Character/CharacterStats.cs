using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;

    [LabelText("角色数据预制件")] public CharacterData_SO characterDataPrefab;

    [HideInInspector] public CharacterData_SO characterData;

    [HideInInspector] public bool isCritical; // 是否暴击

    private void Awake()
    {
        if (characterDataPrefab != null)
            characterData = GameObject.Instantiate(characterDataPrefab);
    }

    #region Combat
    /// <summary>
    /// 造成伤害
    /// </summary>
    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.characterData.currentDefense, 0);
        defener.characterData.currentHealth = Mathf.Max(defener.characterData.currentHealth - damage, 0);
        //Debug.Log($"{defener.gameObject.name} : {defener.characterData.currentHealth}");
        //if (attacker.isCritical)
        //{
        //    defener.GetComponent<Animator>().SetTrigger("Hit");
        //}
        //Update UI 
        defener.UpdateHealthBarOnAttack?.Invoke(defener.characterData.currentHealth, defener.characterData.maxHealth);

        // 测试死亡以后设置窗口的代码
        //if (defener.tag == "Player")
        //{
        //    if (defener.characterData.currentHealth <= 0)
        //    {
        //        Player_Controller.Instance.ChangeState(PlayerState.Dead);
        //    }
        //}

        //Update Exp
        //if (CurrentHealth <= 0)
        //    attacker.characterData.UpdateExp(characterData.killPoint);
    }

    public void TakeDamage(int damage, CharacterStats defener)
    {
        int currentDamage = Mathf.Max(damage - defener.characterData.currentDefense, 0);
        defener.characterData.currentHealth = Mathf.Max(defener.characterData.currentHealth - currentDamage, 0);

        //Update UI
        defener.UpdateHealthBarOnAttack?.Invoke(defener.characterData.currentHealth, defener.characterData.maxHealth);

        //if (CurrentHealth <= 0)
        //    GameManager.Instance.playerStats.characterData.UpdateExp(characterData.killPoint);
        //Debug.Log($"{defener.gameObject.name} : {defener.characterData.currentDefense}");
    }

    /// <summary>
    /// 计算伤害值
    /// </summary>
    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(characterData.minAttack, characterData.maxAttack);

        if (isCritical) coreDamage *= characterData.criticalMultiple; // 判断暴击

        return (int)coreDamage;
    }
    #endregion
}
