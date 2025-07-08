using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/ZombieData", fileName = "ZombieData", order = 1)]
public class ZombieData : ScriptableObject
{
    public float health = 100f; // 좀비의 체력
    public float damage = 20f; // 좀비의 공격력
    public float speed = 2f; // 좀비의 이동 속도
    public Color skinColor = Color.white; // 좀비의 피부 색상
}
