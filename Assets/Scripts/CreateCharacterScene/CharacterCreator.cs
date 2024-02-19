using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
/// <summary>
/// ��ɫ������
/// </summary>
public class CharacterCreator : SingletonMono<CharacterCreator>
{
    [SerializeField] Player_View player_View;
    [SerializeField] Transform characterTransform;
    [SerializeField] Animator animator;

    // ��ְͬҵ��Ԥ������
    [SerializeField] RuntimeAnimatorController[] animatorControllers;

    // ��ְͬҵ������
    [SerializeField] GameObject[] warriorWeapons;
    [SerializeField] GameObject[] assassinWeapons;
    [SerializeField] GameObject[] archerWeapons;
    [SerializeField] GameObject[] tankeWeapons;
    // ��ǰ������
    private GameObject[] currentWeapons;

    public void Init()
    {
        player_View.Init(DataManager.CustomCharacterData);
    }

    /// <summary>
    /// ����ְҵ
    /// </summary>
    public void SetProfession(ProfessionType professionType)
    {
        // ����Ԥ������
        animator.runtimeAnimatorController = animatorControllers[(int)professionType];

        // ��������
        // ���ص�ǰ������
        if (currentWeapons!=null)
        {
            for (int i = 0; i < currentWeapons.Length; i++)
            {
                currentWeapons[i].SetActive(false);
            }
        }

        // ����ְҵȷ����ǰ����������
        switch (professionType)
        {
            case ProfessionType.Warrior:
                currentWeapons = warriorWeapons;
                break;
            case ProfessionType.Assassin:
                currentWeapons = assassinWeapons;
                break;
            case ProfessionType.Archer:
                currentWeapons = archerWeapons;
                break;
            case ProfessionType.Tanke:
                currentWeapons = tankeWeapons;
                break;
        }

        // ��ʾ��ǰ������
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            currentWeapons[i].SetActive(true);
        }

    }

    /// <summary>
    /// ��ת��ɫ
    /// </summary>
    public void RotateCharacter(Vector3 rot)
    {
        characterTransform.Rotate(rot);
    }

    /// <summary>
    /// ���ò�λ
    /// </summary>
    public void SetPart(CharacterPartConfigBase characterPartConfig, bool updateCharacterView)
    {
        player_View.SetPart(characterPartConfig,updateCharacterView);
    }

    public void SetSize(CharacterPartType characterPartType,float size)
    { 
        player_View.SetSize(characterPartType,size);
    }

    public void SetHieght(CharacterPartType characterPartType, float height)
    {
        player_View.SetHeight(characterPartType, height);
    }

    public void SetColor1(CharacterPartType characterPartType, Color color)
    {
        player_View.SetColor1(characterPartType, color);
    }
    public void SetColor2(CharacterPartType characterPartType, Color color)
    {
        player_View.SetColor2(characterPartType, color);
    }

    /// <summary>
    /// ��ȡ��ǰ�Ľ�ɫ����
    /// </summary>
    public CharacterPartConfigBase GetCurrentPartConfig(CharacterPartType characterPartType)
    {
        return player_View.GetCurrentPartConfig(characterPartType);
    }
}
