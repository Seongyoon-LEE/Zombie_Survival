using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Photon.Pun; // Photon Pun 네트워킹 라이브러리

public class HealthPack : MonoBehaviourPun, IItem
{
    public int health = 50; // 회복할 체력
    public void Use(GameObject target)
    {
        // 전달 받은 게임 오브젝트로부터 LivingEntity 컴포넌트 가져오기 시도
        LivingEntity life = target.GetComponent<LivingEntity>();

        // LivingEntity 컴포넌트가 있따면
        if(life != null)
        {
            // 체력을 증가시킵니다
            life.RestoreHealth(health);
        }
        // 모든 클라이언트에서의 자신을 파괴
        PhotonNetwork.Destroy(gameObject);
    }
}
