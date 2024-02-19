using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ����_ScriptObject
/// </summary>
[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("State Info")]

    [LabelText("�������")] public int maxHealth;

    [LabelText("��ǰ����")] public int currentHealth;

    [LabelText("��������")] public int baseDefense;

    [LabelText("��ǰ����")] public int currentDefense;

    [Header("Combat")]

    [LabelText("��С����")] public int minAttack;

    [LabelText("��󹥻�")] public int maxAttack;

    [LabelText("������")] public float criticalChance;

    [LabelText("�����ӳɰٷֱ�")] public float criticalMultiple;

    [LabelText("�չ���ȴ")] public float baseAttackCoolDown;

    [LabelText("�չ���Χ")] public float baseAttackRange;

    [LabelText("���м���")] public List<SkillData> holdSkills; // ��ʱ����Ҫ

    [Header("Kill")]

    [LabelText("��ɱ���侭��ֵ")] public int killExp;

    [Header("Level")]

    [LabelText("��ǰ�ȼ�")] public int currentLevel;

    [LabelText("���ȼ�")] public int maxLevel;

    [LabelText("��ǰ�ȼ�������ֵ")] public int baseExp;

    [LabelText("��ǰ����")] public int currentExp;

    [LabelText("����Buff")] public float levelBuff;

    [LabelText("���������Ա䶯����")]
    public float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;

        if (currentExp >= baseExp)
            LevelUp();
    }

    public void LevelUp()
    {
        // ������������������
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, maxLevel);
        baseExp += (int)(baseExp * LevelMultiplier);

        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;

        Debug.Log($"Level Up! {currentLevel} Max Health:{maxHealth}");
    }
}
