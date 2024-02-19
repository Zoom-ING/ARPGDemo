using Cinemachine;
using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistyValleySceneManager : SingletonMono<MistyValleySceneManager>
{
    [SerializeField] private Transform playerSpownPoint;

    #region �����߼�
    public bool IsTest;
    public bool IsCreateArchive;
    #endregion
    private void Start()
    {
        #region �����߼�
        if (IsTest)
        {
            if (IsCreateArchive)
            {
                DataManager.CreateArchive();
            }
            else
            {
                DataManager.LoadCurrentArchive();
            }
        }
        #endregion

        // ��ʼ����ɫ
        GameObject player = ResSystem.InstantiateGameObject("Player");
        CinemachineFreeLook freeCamera = FindObjectOfType<CinemachineFreeLook>();
        freeCamera.Follow = Player_Controller.Instance.transform;
        freeCamera.LookAt = Player_Controller.Instance.transform;

        Player_Controller.Instance.Init();
        Player_Controller.Instance.transform.position = playerSpownPoint.position;


    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        Debug.Log(Player_Controller.Instance.CharacterController.ToString());
    //    }
    //}

    //private void Start()
    //{
    //    // ��ʼ����ɫ
    //    Player_Controller.Instance.Init();
    //}
}
