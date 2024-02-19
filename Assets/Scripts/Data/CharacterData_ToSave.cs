using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData_ToSave
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
}
