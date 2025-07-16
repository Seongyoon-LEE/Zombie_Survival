using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Photon Pun 네트워킹 라이브러리
public class AmmoPack :MonoBehaviourPun, IItem
{
    public int ammo = 30;
    public void Use(GameObject target)
    { 
        // 전달 받은 게임 오브젝트로부터 PlayerShooter 컴포넌트를 가져오기 시도
     PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();

        // PlayerShooter 컴포넌트가 존재하고, 총 오브젝트가 존재하면
        if(playerShooter != null && playerShooter.gun != null)
        {
            // 총의 남은 탄환 수를 ammo 만큼 더합니다

            playerShooter.gun.photonView.RPC("AddAmmo", RpcTarget.All, ammo);
        }
        // 모든 클라이언트에서의 자신을 파괴
        PhotonNetwork.Destroy(gameObject);
    }
}
