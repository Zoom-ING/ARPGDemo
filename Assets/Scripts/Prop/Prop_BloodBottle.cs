using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Ѫƿ����
/// </summary>
public class Prop_BloodBottle : Prop_Base
{
    private UIBloodBuffData_SO data;
    [LabelText("��������")] private float BloodBuff;

    private CharacterData_SO playerData => Player_Controller.Instance.stats.characterData;

    private void Awake()
    {
        if (DataPrefab != null)
            data = GameObject.Instantiate(DataPrefab) as UIBloodBuffData_SO;
        BloodBuff = data.BloodBuff;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //TODO: �����������û��,ʰȡ

            // ʰȡ ʹ�ñ������������ٿر����������ݵ����Ӻ�ɾ��
            bool packUpSuccess = PackageUtil.AddProp(data.propStats);

            // TODO:ûʰȡ�ɹ�����Ūһ��������Ч
            AudioSystem.PlayOneShot(PickUpAudio, transform.position);

            if (packUpSuccess)
            {
                UI_PackagePanelWindow packageWindow = UISystem.GetWindow<UI_PackagePanelWindow>();
                if (packageWindow != null)//����򿪱���ʱ��ʰȡ�˵��ߣ�ˢ�±���
                    packageWindow.UpdateProp(data.propStats);

                // AudioSystem.PlayOneShot(PickUpAudio, transform.position);
                Destroy(gameObject);

                UISystem.AddTips($"ʰȡ{data.propStats.Name}:{data.BloodBuff}");


            }
            else
            {
                UISystem.AddTips($"{data.propStats.Name}���ˣ�ʰȡʧ��");
            }
        }
    }

    /// <summary>
    /// ʹ��Ѫƿ ���ɸ��෽����
    /// </summary>
    private void UseBloodBottle()
    {
        int bloodVolume = (int)(playerData.maxHealth * BloodBuff);
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth + bloodVolume, 0, playerData.maxHealth);

        // ��������..
        PackageUtil.RemoveProp(data.propStats, 1);
    }
}
