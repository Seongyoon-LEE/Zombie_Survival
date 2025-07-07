using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum eState
    {
        READY, // 대기 상태
        FIRE, // 발사 상태
        RELOAD, // 재장전 상태 
        EMPTY // 총알이 없는 상태

    }
    public eState state { get; private set; } = eState.READY; // 현재 총의 상태를 나타내는 변수, 기본값은 READY 
    public GunData gunData; // 총 데이터 스크립터블 오브젝트

    [SerializeField] Transform firePos; // 총알이 발사되는 위치
    [SerializeField] ParticleSystem muzzleFlashEffect; // 총구 플래시 이펙트
    [SerializeField] ParticleSystem shellEjectEffect; // 탄피 배출 이펙트
    [SerializeField] LineRenderer lineRenderer;
    AudioSource source;
    float fireDistance = 100f; // 총알이 날아가는 거리(사정거리)

    [SerializeField] int ammoRemain; // 현재 남아있는 총알 수
    [SerializeField] int magAmmo; // 현재 탄창에 있는 총알 수
    float lastFireTime; // 마지막 발사 시간
    Vector3 hitPosition; // 총알이 맞은 위치
    WaitForSeconds wsShotEffect; // 발사 이펙트 대기 시간
    WaitForSeconds wsReloadTime;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // 라인 렌더러의 포지션 개수를 2로 설정 (시작점과 끝점)
        lineRenderer.enabled = false; // 초기에는 라인 렌더러 비활성화
        wsShotEffect = new WaitForSeconds(0.03f); // 발사 이펙트 대기 시간 설정 (0.03초)
        wsReloadTime = new WaitForSeconds(gunData.reloadTime);
    }
    
    private void OnEnable()
    {
     ammoRemain = gunData.startAmmoRemain; // 전체 총알 수 초기화
        magAmmo = gunData.magCapacity; // 탄창 총알 수 초기화
        state = eState.READY; // 상태를 READY로 설정
        lastFireTime = 0f; // 마지막 발사 시간 초기화
    }
    public void Fire() // 발사 
    {
        if(state == eState.READY && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time; // 마지막 발사 시간 갱신
            Shot(); // 실제 발사 처리 호출
        }
    }
    void Shot() // 실제 발사 처리 
    {
        RaycastHit hit; // 레이캐스트 히트 정보
        Vector3 hitPos = Vector3.zero; // 맞은 위치 초기화
        if (Physics.Raycast(firePos.position, firePos.forward, out hit, fireDistance)) // 레이캐스트 발사
        {   // 느슨한 카풀링 <중요>
            IDamageable target = hit.collider.GetComponent<IDamageable>(); // 충돌한 오브젝트에서 IDamageable 인터페이스를 찾음
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal); // 충돌한 오브젝트가 IDamageable을 구현하고 있다면 데미지 처리 
            }
            hitPos = hit.point; // 맞은 위치 설정
        }
        else
        {   // 위치 + 방향 * 거리 
            hitPos = firePos.position + firePos.forward * fireDistance; // 맞은 위치가 없으면 사정거리 끝으로 설정
        }
        StartCoroutine(ShotEffect(hitPos)); // 발사 이펙트 코루틴 시작
        magAmmo--; // 탄창의 총알 수 감소
        if (magAmmo <= 0)
        {
            state = eState.EMPTY; 
        }
    }
            IEnumerator ShotEffect(Vector3 hitPosition) // 발사 이펙트 코루틴
            {
                muzzleFlashEffect.Play(); // 총구 플래시 이펙트 재생
                shellEjectEffect.Play(); // 탄피 배출 이펙트 재생
                source.PlayOneShot(gunData.shotClip); // 발사 사운드 재생

                lineRenderer.SetPosition(0, firePos.position); // 라인 렌더러 시작점 설정
                lineRenderer.SetPosition(1, hitPosition); // 라인 렌더러 끝점 설정
                lineRenderer.enabled = true; // 라인 렌더러 활성화
                yield return wsShotEffect; // 0.03초 대기
                lineRenderer.enabled = false; // 라인 렌더러 비활성화
            }
    public bool Reload() // 재장전 시도
    {
        if(state == eState.RELOAD || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            return false; // 이미 재장전 중이거나, 남은 총알이 없거나, 탄창이 가득 찼다면 재장전 불가
        }
        StartCoroutine(ReloadRoutine()); // 재장전 코루틴 시작
        return true;
    }
    IEnumerator ReloadRoutine() // 재장전 코루틴
    {
        state = eState.RELOAD; // 상태를 RELOAD로 변경
        source.PlayOneShot(gunData.reloadClip); // 재장전 사운드 재생
     
        yield return wsReloadTime; // 재장전 시간 대기
                                   // 탄창에 채울 탄알 계산
        int ammoToFill = gunData.magCapacity - magAmmo; // 채워야 할 총알 수 계산

        // 탄창에 채워야할 탄알이 남은 탄알보다 많다면 채워야할 탄알 수를 남은 탄알수에 맞춰 조정 
        if (ammoRemain < ammoToFill) // 남은 총알이 채워야 할 총알 수보다 적다면
        {
            ammoToFill = ammoRemain;
        }
        magAmmo += ammoToFill; // 탄창에 총알 채우기
        ammoRemain -= ammoToFill; // 남은 총알 수 감소
        state = eState.READY; // 상태를 발사 준비 상태로 변경
    }
}
