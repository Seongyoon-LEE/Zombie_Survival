using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 포톤 네트워크 관련 라이브러리

public class PlayerInput : MonoBehaviourPun
{
    [Header("Input Axis Names")]
    public string moveAxisName = "Vertical"; // 앞뒤 이동 입력
    public string rotateAxisName = "Horizontal"; // 좌우 회전 입력
    public string fireButtonName = "Fire1"; // 공격 입력
    public string reloadButtonName = "Reload"; // 재장전 입력

    // 프로퍼티 입력값 제공
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }

    void Update()
    {
        // 로컬 플레이어가 아닌 경우 입력을 받지 않음
        if (!photonView.IsMine) return; 

        if (GameManager.instance != null && GameManager.instance.isGameOver)
        {
            move = 0f; rotate = 0f; fire = false; reload = false;
            return; // 게임 오버 상태면 입력 감지 중지
        }
        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButton(reloadButtonName);
    }
}
