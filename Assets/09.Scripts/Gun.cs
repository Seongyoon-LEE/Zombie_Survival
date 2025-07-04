using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum eState
    {
        READY, // ��� ����
        FIRE, // �߻� ����
        RELOAD // ������ ���� 
    }
    public GunData gunData; // �� ������ ��ũ���ͺ� ������Ʈ
    
    [SerializeField] AudioSource source;
    [SerializeField] LineRenderer lineRenderer;

    public float damage = 25f;
    public int magCapacity = 25; // źâ �뷮
    public float timeBetFire = 0.1f; // �߻� ����
    public float reloadTime = 1.8f; // ������ �ð� 


    void Start()
    {
        source = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
    }


    void Update()
    {
        
    }
}
