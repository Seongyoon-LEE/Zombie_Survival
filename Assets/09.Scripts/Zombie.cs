using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : LivingEntity
{
    public LayerMask whatIsTarget; // 추적 대상 레이어
    private LivingEntity targetEntity; // 추적 대상 엔티티

    public ParticleSystem hitEffect; // 적중 효과 파티클
    public AudioClip hitClip; // 적중 사운드 클립
    public AudioClip deathClip; // 죽음 사운드 클립
    private AudioSource source; // 오디오 소스 컴포넌트
    private Animator animator; // 애니메이터 컴포넌트
    private NavMeshAgent agent; // 네비게이션 메쉬 에이전트
    private MeshRenderer meshRenderer; // 메쉬 렌더러 컴포넌트

    public float damage = 20f; // 피격 데미지
    public float timeBetAttack = 0.5f; // 공격 간격
    private float lastAttackTime; // 마지막 공격 시간

    WaitForSeconds wsUpdatePath = new WaitForSeconds(0.25f); // 경로 업데이트 대기 시간
    readonly int hashHasTarget = Animator.StringToHash("HasTarget"); // 애니메이터 파라미터 해시
    readonly int hashDie = Animator.StringToHash("Die"); // 죽음 애니메이터 트리거 해시

    private bool hasTarget
    {
        get
        {
            if(targetEntity != null && !targetEntity.dead) // 추적 대상이 null이 아니고 죽지 않은 상태인지 확인
            {
                return true; // 유효한 추적 대상이 있음
            }
            return false; // 유효한 추적 대상이 없음
        }
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>(); // 네비게이션 메쉬 에이전트 초기화
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 초기화
        source = GetComponent<AudioSource>(); // 오디오 소스 컴포넌트 초기화
        meshRenderer = GetComponent<MeshRenderer>(); // 메쉬 렌더러 컴포넌트 초기화
    }
    public void Setup(ZombieData zombieData) // 좀비 AI 초기 스펙을 결정하는 셋업 메서드
    {
        startingHealth = zombieData.health; // 초기 체력 설정
        health = startingHealth; // 현재 체력 초기화
        damage = zombieData.damage; // 공격력 설정
        agent.speed = zombieData.speed; // 이동 속도 설정
        meshRenderer.material.color = zombieData.skinColor; // 피부 색상 설정
    }
    private void Start()
    {
        StartCoroutine(UpdatePath()); // 경로 업데이트 코루틴 시작
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetBool(hashHasTarget, hasTarget); // 추적 대상이 있는지 애니메이터에 전달
    }
    IEnumerator UpdatePath()
    {
        while(!dead)
        {
            if(hasTarget)
            {
                agent.isStopped = false; // 추적 대상이 있으면 이동 가능
                agent.SetDestination(targetEntity.transform.position); // 추적 대상 위치로 이동 경로 설정
            }
            else
            {
                agent.isStopped = true; // 추적 대상이 없으면 이동 정지
                // 20 유닛 범위 내의 가상의 구를 그렸을때 구와 겹치는 모든 콜라이더를 가져옴
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
                for(int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>(); // 콜라이더에서 LivingEntity 컴포넌트 가져오기
                    if(livingEntity != null && !livingEntity.dead) // LivingEntity가 있고 죽지 않은 경우 
                    {
                        targetEntity = livingEntity; // 추적 대상 설정
                        break; // 첫번째 추적 대상만 설정하고 루프 종료
                    }
                }
            }
            yield return wsUpdatePath; // 0.25초마다 경로 업데이트
        }
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(!dead)
        {
            hitEffect.transform.position = hitPoint; // 피격 이펙트 위치 설정
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal); // 피격 이펙트 회전 설정
            hitEffect.Play(); // 피격 이펙트 재생
            source.PlayOneShot(hitClip); // 피격 사운드 재생
        }

        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public override void Die()
    {
     
        // 다른 AI를 방해하지 않도록 자신의 모든 콜라이더를 비활성화
        Collider[] zombiecolliders = GetComponents<Collider>(); // 좀비의 모든 콜라이더 가져오기
        for (int i = 0; i < zombiecolliders.Length; i++)
        {
            zombiecolliders[i].enabled = false; // 좀비의 콜라이더 비활성화
        }
        agent.isStopped = true; // 네비게이션 에이전트 정지
        agent.enabled = false; // 네비게이션 에이전트 비활성화
        animator.SetTrigger(hashDie); // 죽음 애니메이션 트리거 설정'
        source.PlayOneShot(deathClip); // 죽음 사운드 재생
        base.Die();
    }
    public void OnTriggerStay(Collider other) // 트리거 안에 있을때 특정 기능을 유지할때
    {
        // 트리거가 충돌한 상대방 게임 오브젝트가 추적 대상이라면 공격 실행
        // 사망하지 않았고 최근 공격시점에서 timeBetAttack 시간 이상 경과했을 때 공격 가능 
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity attackTarget = other.GetComponent<LivingEntity>(); // 충돌한 상대방 게임 오브젝트에서 LivingEntity 컴포넌트 가져오기
            // 상대방의 LivingEntity가 자신의 추적 대상이라면 공격 실행 
            if(attackTarget != null && attackTarget == targetEntity)
            {
                lastAttackTime = Time.time; // 마지막 공격 시간 갱신
                Vector3 hitPoint = other.ClosestPoint(transform.position); // 충돌 지점 계산
                // 상대방의 피격위치와 피격 방향을 근사값으로 계산
                Vector3 hitNormal = transform.position - other.transform.position;
                attackTarget.OnDamage(damage, hitPoint, hitNormal); // 상대방에게 데미지 적용
            }
        }
    }

}
