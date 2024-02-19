using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTower : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.Tab))
        {
            UISystem.Show<UI_LevelPrepareWindow>().Init();
            Time.timeScale = 0f;
        }
    }
}
