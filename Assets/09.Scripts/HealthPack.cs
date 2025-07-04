using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : IItem
{
    public int health = 50;
    public void Use(GameObject target)
    {
        // 체력을 증가하는 처리
        Debug.Log($"체력이 회복되었다 : {health}");
    }
}
