using Sirenix.OdinInspector;
using System;
using System.Runtime.Serialization;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<int, int> UpdateHealthBarOnAttack;

    [LabelText("��ɫ����Ԥ�Ƽ�")] public CharacterData_SO characterDataPrefab;

    [HideInInspector] public CharacterData_SO characterData;

    [HideInInspector] public bool isCritical; // �Ƿ񱩻�

    private void Awake()
    {
        if (characterDataPrefab != null)
            characterData = GameObject.Instantiate(characterDataPrefab);
    }

    #region Combat
    /// <summary>
    /// ����˺�
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

        // ���������Ժ����ô��ڵĴ���
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
    /// �����˺�ֵ
    /// </summary>
    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(characterData.minAttack, characterData.maxAttack);

        if (isCritical) coreDamage *= characterData.criticalMultiple; // �жϱ���

        return (int)coreDamage;
    }
    #endregion
}
