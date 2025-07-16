using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun; // 포톤 네트워크 관련 라이브러리
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
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 초기화
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
    [PunRPC] // 포톤 네트워크에서 원격 프로시저 호출을 위한 어트리뷰트
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
    [PunRPC]
    public override void RestoreHealth(float newHealth)
    {
        base.RestoreHealth(newHealth); // LivingEntity의 RestoreHealth 호출
        // 추가적인 플레이어 전용 로직이 있다면 여기에 작성
        healthSlider.value = health; // 슬라이더의 값 업데이트
    }
    public override void Die()
    {
        UIManager.instance.SetActiveGameoverUI(true); // 게임 오버 UI 활성화

        base.Die(); // LivingEntity의 Die 호출
        // 플레이어가 죽었을 때 추가적인 로직이 있다면 여기에 작성
        Debug.Log("Player has died.");
        healthSlider.gameObject.SetActive(false); // 슬라이더 UI 비활성화
        source.PlayOneShot(deathClip); // 죽었을 때 사운드 재생
        animator.SetTrigger(hashDie); // 애니메이션 트리거 설정
        movement.enabled = false; // 플레이어 이동 비활성화

        //Invoke("Respawn", 3f); // 3초 후에 Respawn 메서드 호출

    }
    private void OnTriggerEnter(Collider other) // isTrigger 체크시
    {
        if(!dead)
        {
            // 느슨한 카풀링
            IItem item = other.GetComponent<IItem>(); // IItem 인터페이스를 구현한 컴포넌트 가져오기
            
            if (item != null) // 아이템이 있다면
            {
                // 호스트만 아이템 직접 사용 가능
                // 호스트에서는 아이템을 사용후, 사용된 아이템의 효과를 모든 클라이언트들에게 동기화
                if(PhotonNetwork.IsMasterClient)
                {
                    item.Use(gameObject); // 아이템 사용
                }

                source.PlayOneShot(itemPickUpClip); // 아이템 획득 사운드 재생
            }
        }
    }
    // 부활 처리 
    public void Respawn()
    {
        // 로컬 플레이어만 직접 위치를 변경 가능
        if(photonView.IsMine)
        {
            // 원점에서 반경 5유닛  내부의 랜덤한 위치 지정
            Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
            // 랜덤 위치의 y값을 0으로 변경
            randomSpawnPos.y = 0f;

            // 지정된 랜덤 위치로 이동
            transform.position = randomSpawnPos;
        }
        // 컴포넌트들을 리셋하기 위해 게임 오브젝트를 잠시 껐다가 다시 켜기
        // 컴포넌트들의 OnDisable(), OnEnable() 메서드가 호출됨
        gameObject.SetActive(false); // 게임 오브젝트 비활성화
        gameObject.SetActive(true); // 게임 오브젝트 활성화

    }
}

