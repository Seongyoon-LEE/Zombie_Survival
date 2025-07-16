using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviourPun, IItem
{
    public int socre = 200; // 증가할 점수 
    public void Use(GameObject target)
    { 
        // 게임 매니저로 접근해 점수 추가
        GameManager.instance.AddScore(socre);

        // 모든 클라이너트에서의 자신을 파괴 
       PhotonNetwork.Destroy(gameObject);
    }
}
