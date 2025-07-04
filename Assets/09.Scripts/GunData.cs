using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "ScriptableObjects/GunData")]
public class GunData : ScriptableObject
{
    [SerializeField] AudioClip shotClip;
    [SerializeField] AudioClip reloadClip;
    [SerializeField] float damage = 25f;
    [SerializeField] int magCapacity = 25;
    [SerializeField] int startAmmoRemain = 100; // 처음 주어지는 전체 탄약수
    [SerializeField] float timeBetFire = 0.1f; // 발사 간격
    [SerializeField] float reloadTime = 1.8f; // 재장전 시간 
}
