using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun; // Cinemachine 라이브러리 추가

public class CameraSetup : MonoBehaviourPun
{
    void Start()
    {
        if (photonView.IsMine)


        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.Follow = transform; // 플레이어 오브젝트를 따라다니도록 설정
            followCam.LookAt = transform; // 플레이어 오브젝트를 바라보도록 설정
        }
    }

}
