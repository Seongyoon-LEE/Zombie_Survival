using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HealthPack : MonoBehaviour, IItem
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
        // 사용 되었으므로, 자신을 파괴
        Destroy(gameObject);
    }
}
