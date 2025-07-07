using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Hardware;
using UnityEngine;
// 부몸 클래스 IDamageable을 상속 받는 LivingEntity 클래스 -> Player.Enemy 등 생명체의 공통 기능을 구현 
// 이전 로직에서는 플레이어나 적 캐릭터의 죽는 과정이 데미지 수치만 다를 뿐 동일한 로직으로 처리 되었음
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHealth = 100f; // 시작 생명력
    public float health { get; protected set; } // 현재 생명력
    public bool dead { get; protected set; } // 생명체가 죽었는지 여부
    public event Action onDeath; // 죽음 이벤트 // Action -> 반환하지 않는 메서드를 대리
    
    protected virtual void OnEnable()
    {
        dead = false; // 생명체가 활성화 될 때 죽지 않은 상태로 초기화
        health = startingHealth; // 생명력을 시작 생명력으로 초기화
    }
    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage; // 데미지를 받아 생명력 감소
        if(health <= 0f && !dead) // 생명력이 0 이하이고 죽지 않은 상태라면
        {
            Die(); // 죽음 처리 메서드 호출
        }
    }
    public virtual void RestoreHealth(float newHealth)
    {
        if(dead) // 죽은 상태라면 
        {
            return; // 체력 회복하지 않음
        }
        health += newHealth; // HealthPack 체력 회복 수치만큼 체력 증가 
    }
    public virtual void Die()
    {
        dead = true;
        if(onDeath != null) // 죽음 이벤트가 구독되어 있다면
        {
            onDeath(); // 이벤트 호출
        }
        gameObject.SetActive(false); // 게임 오브젝트 비활성화
    }
}
