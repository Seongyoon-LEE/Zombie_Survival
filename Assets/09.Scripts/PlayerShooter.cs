using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] Gun gun; // 총 스크립트 참조
    [SerializeField] Transform gunPivot; // 총의 회전 중심이 되는 피벗
    [SerializeField] Transform leftHandleMount; // 왼손이 잡는 위치
    [SerializeField] Transform rightHandleMount; // 오른손이 잡는 위치

    PlayerInput playerInput; // 플레이어 입력 스크립트 참조
    Animator animator; // 애니메이터 컴포넌트 참조

    readonly int hashReload = Animator.StringToHash("Reload"); // 애니메이터 트리거 해시
    private void OnEnable()
    {
        //gun.gameObject.SetActive(true); // 총 오브젝트 활성화
    }
    void Start()
    {
        playerInput = GetComponent<PlayerInput>(); // PlayerInput 컴포넌트 가져오기
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        gun = GetComponentInChildren<Gun>(); // 자식 오브젝트에서 Gun 컴포넌트 가져오기
    }

 
    void Update() // 입력을 감지 하고 총을 발사 하거나 재장전
    {
        if(playerInput.fire)
        {
            gun.Fire();
        }
        else if(playerInput.reload)
        {
            if(gun.Reload()) // 총이 재장전 가능하면
            {
                animator.SetTrigger(hashReload); // 애니메이터에 재장전 트리거 설정
            }
            
        }
        UpdateUI(); // UI 업데이트 호출
    }
    void UpdateUI()
    {
        // 탄약 UI 업데이트 

    }
    private void OnAnimatorIK(int layerIndex)
    {
        // 애니메이션 IK를 사용하여 손 위치 조정 애니메이터의 실시간 IK 업데이트 
        gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f); // 오른손 IK 위치 가중치 설정
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f); // 오른손 IK 회전 가중치 설정

        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandleMount.position); // 오른손 위치 설정
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandleMount.rotation); // 오른손 회전 설정

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f); // 왼손 IK 위치 가중치 설정
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f); // 왼손 IK 회전 가중치 설정

        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandleMount.position); // 왼손 위치 설정
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandleMount.rotation); // 왼손 회전 설정

    }
    private void OnDisable()
    {
        gun.gameObject.SetActive(false); // 총 오브젝트 비활성화
    }
}
