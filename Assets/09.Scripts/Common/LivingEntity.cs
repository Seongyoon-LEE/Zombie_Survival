using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Hardware;
using UnityEngine;
using Photon.Pun; // 포톤 네트워크 관련 라이브러리
// 부모 클래스 IDamageable을 상속 받는 LivingEntity 클래스 -> Player.Enemy 등 생명체의 공통 기능을 구현 
// 이전 로직에서는 플레이어나 적 캐릭터의 죽는 과정이 데미지 수치만 다를 뿐 동일한 로직으로 처리 되었음
public class LivingEntity : MonoBehaviourPun, IDamageable
{
    public float startingHealth = 100f; // 시작 생명력
    public float health { get; protected set; } // 현재 생명력
    public bool dead { get; protected set; } // 생명체가 죽었는지 여부
    public event Action onDeath; // 죽음 이벤트 // Action -> 반환하지 않는 메서드를 대리

    [PunRPC]
    public void ApplyUpdatedHealth(float newHealth, bool newDead)
    {
        health  = newHealth; // 새로운 생명력 값으로 업데이트
        dead = newDead; // 새로운 죽음 상태로 업데이트
    }
    protected virtual void OnEnable()
    {
        dead = false; // 생명체가 활성화 될 때 죽지 않은 상태로 초기화
        health = startingHealth; // 생명력을 시작 생명력으로 초기화
    }
    [PunRPC]
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(PhotonNetwork.IsMasterClient) // 마스터 클라이언트에서만 생명력 업데이트
        {
            health -= damage; // 데미지를 받아 생명력 감소
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead); // 호스트에서 클라잉너트로 동기화 
            photonView.RPC("OnDamage", RpcTarget.Others, damage, hitPoint, hitNormal); // 다른 클라이언트에게도 데미지 적용
        }
        if(health <= 0f && !dead) // 생명력이 0 이하이고 죽지 않은 상태라면
        {
            Die(); // 죽음 처리 메서드 호출
        }
    }
    [PunRPC]
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead) // 죽은 상태라면 
        {
            return; // 체력 회복하지 않음
        }
        if(PhotonNetwork.IsMasterClient) // 마스터 클라이언트에서만 생명력 업데이트
        {
            health += newHealth; // 체력 회복
            photonView.RPC("ApplyUpdatedHealth", RpcTarget.Others, health, dead); // 호스트에서 클라이언트로 동기화
            photonView.RPC("RestoreHealth", RpcTarget.Others, newHealth); // 다른 클라이언트에게도 체력 회복 적용
        }
    }
    public virtual void Die()
    {
        dead = true;
        if(onDeath != null) // 죽음 이벤트가 구독되어 있다면
        {
            onDeath(); // 이벤트 호출
        }
        //gameObject.SetActive(false); // 게임 오브젝트 비활성화
    }
}
