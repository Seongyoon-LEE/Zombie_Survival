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
    [SerializeField] int startAmmoRemain = 100; // ó�� �־����� ��ü ź���
    [SerializeField] float timeBetFire = 0.1f; // �߻� ����
    [SerializeField] float reloadTime = 1.8f; // ������ �ð� 
}
