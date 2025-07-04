using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum eState
    {
        READY, // 대기 상태
        FIRE, // 발사 상태
        RELOAD // 재장전 상태 
    }
    public GunData gunData; // 총 데이터 스크립터블 오브젝트
    
    [SerializeField] AudioSource source;
    [SerializeField] LineRenderer lineRenderer;

    public float damage = 25f;
    public int magCapacity = 25; // 탄창 용량
    public float timeBetFire = 0.1f; // 발사 간격
    public float reloadTime = 1.8f; // 재장전 시간 


    void Start()
    {
        source = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
    }


    void Update()
    {
        
    }
}
