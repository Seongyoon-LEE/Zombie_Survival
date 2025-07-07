using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : LivingEntity
{
    public Slider healthSlider; // 플레이어의 체력을 표시할 슬라이더 UI
    public AudioClip deathClip; // 플레이어가 죽었을 때 재생할 오디오 클립
    public AudioClip hitClip; // 플레이어가 데미지를 받았을 때 재생할 오디오 클립
    public AudioClip itemPickUpClip; // 플레이어가 아이템을 획득했을 때 재생할 오디오 클립
    public AudioSource source; // 오디오 소스 컴포넌트
    Animator animator; // 플레이어의 애니메이션을 관리하는 컴포넌트

    PlayerMovement movement; // 플레이어의 이동을 관리하는 컴포넌트
    PlayerShooter shooter; // 플레이어의 총기 조작을 관리하는 컴포넌트

    readonly int hashDie = Animator.StringToHash("Die"); // 애니메이션 트리거 해시

    private void Awake()
    {
        source = GetComponent<AudioSource>(); // 오디오 소스 컴포넌트 초기화
        movement = GetComponent<PlayerMovement>(); // PlayerMovement 컴포넌트 초기화
        shooter = GetComponent<PlayerShooter>(); // PlayerShooter 컴포넌트 초기화
    }
    protected override void OnEnable() // 확장성 
    {
        base.OnEnable(); // LivingEntity의 OnEnable 호출
        healthSlider.gameObject.SetActive(true); // 현재 슬라이더 UI 활성화
        healthSlider.maxValue = startingHealth; // 슬라이더의 최대값을 시작 생명력으로 설정
        healthSlider.value = health; // 슬라이더의 값 초기화

        movement.enabled = true; // 플레이어 이동 활성화
        shooter.enabled = true;// 플레이어 총기 조작 활성화
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    { 
        // 추가적인 플레이어 전용 로직이 있다면 여기에 작성
        if(!dead) // 플레이어가 죽지 않은 상태라면
        {
            source.PlayOneShot(hitClip); // 데미지를 받았을 때 히트 사운드 재생
        }
        base.OnDamage(damage, hitPoint, hitNormal); // LivingEntity의 OnDamage 호출
        healthSlider.value = health; // 슬라이더의 값 업데이트
    }
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth); // LivingEntity의 RestoreHealth 호출
        // 추가적인 플레이어 전용 로직이 있다면 여기에 작성
        healthSlider.value = health; // 슬라이더의 값 업데이트
    }
    public override void Die()
    {
        base.Die(); // LivingEntity의 Die 호출
        // 플레이어가 죽었을 때 추가적인 로직이 있다면 여기에 작성
        Debug.Log("Player has died.");
        healthSlider.gameObject.SetActive(false); // 슬라이더 UI 비활성화
        source.PlayOneShot(deathClip); // 죽었을 때 사운드 재생
        animator.SetTrigger(hashDie); // 애니메이션 트리거 설정
        movement.enabled = false; // 플레이어 이동 비활성화

    }
    private void OnTriggerEnter(Collider other) // isTrigger 체크시
    {
        if(!dead)
        {
            IItem item = other.GetComponent<IItem>(); // IItem 인터페이스를 구현한 컴포넌트 가져오기
            if (item != null) // 아이템이 있다면
            {

                item.Use(gameObject); // 아이템 사용
                source.PlayOneShot(itemPickUpClip); // 아이템 획득 사운드 재생
            }
        }
    }
}

