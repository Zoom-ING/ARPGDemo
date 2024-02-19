using JKFrame;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 血瓶道具
/// </summary>
public class Prop_BloodBottle : Prop_Base
{
    private UIBloodBuffData_SO data;
    [LabelText("属性增益")] private float BloodBuff;

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
            //TODO: 如果背包容量没满,拾取

            // 拾取 使用背包工具类来操控背包道具内容的增加和删除
            bool packUpSuccess = PackageUtil.AddProp(data.propStats);

            // TODO:没拾取成功可以弄一个其他音效
            AudioSystem.PlayOneShot(PickUpAudio, transform.position);

            if (packUpSuccess)
            {
                UI_PackagePanelWindow packageWindow = UISystem.GetWindow<UI_PackagePanelWindow>();
                if (packageWindow != null)//如果打开背包时，拾取了道具，刷新背包
                    packageWindow.UpdateProp(data.propStats);

                // AudioSystem.PlayOneShot(PickUpAudio, transform.position);
                Destroy(gameObject);

                UISystem.AddTips($"拾取{data.propStats.Name}:{data.BloodBuff}");


            }
            else
            {
                UISystem.AddTips($"{data.propStats.Name}满了，拾取失败");
            }
        }
    }

    /// <summary>
    /// 使用血瓶 做成父类方法？
    /// </summary>
    private void UseBloodBottle()
    {
        int bloodVolume = (int)(playerData.maxHealth * BloodBuff);
        playerData.currentHealth = Mathf.Clamp(playerData.currentHealth + bloodVolume, 0, playerData.maxHealth);

        // 数量减少..
        PackageUtil.RemoveProp(data.propStats, 1);
    }
}
