using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData_ToSave
{
    [Header("State Info")]

    [LabelText("最大生命")] public int maxHealth;

    [LabelText("当前生命")] public int currentHealth;

    [LabelText("基础防御")] public int baseDefense;

    [LabelText("当前防御")] public int currentDefense;

    [Header("Combat")]

    [LabelText("最小攻击")] public int minAttack;

    [LabelText("最大攻击")] public int maxAttack;

    [LabelText("暴击率")] public float criticalChance;

    [LabelText("暴击加成百分比")] public float criticalMultiple;

    [LabelText("普攻冷却")] public float baseAttackCoolDown;

    [LabelText("普攻范围")] public float baseAttackRange;

    [LabelText("持有技能")] public List<SkillData> holdSkills; // 暂时不需要

    [Header("Kill")]

    [LabelText("击杀掉落经验值")] public int killExp;

    [Header("Level")]

    [LabelText("当前等级")] public int currentLevel;

    [LabelText("最大等级")] public int maxLevel;

    [LabelText("当前等级满经验值")] public int baseExp;

    [LabelText("当前经验")] public int currentExp;

    [LabelText("升级Buff")] public float levelBuff;
}
