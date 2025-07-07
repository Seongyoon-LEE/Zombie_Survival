using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "ScriptableObjects/GunData")]
public class GunData : ScriptableObject
{
    public AudioClip shotClip;
    public AudioClip reloadClip;
    public float damage = 25f;
    public int magCapacity = 25;
    public int startAmmoRemain = 100; // 처음 주어지는 전체 탄약수
    public float timeBetFire = 0.1f; // 발사 간격
    public float reloadTime = 1.8f; // 재장전 시간 
}
