using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : IItem
{
    public int health = 50;
    public void Use(GameObject target)
    {
        // ü���� �����ϴ� ó��
        Debug.Log($"ü���� ȸ���Ǿ��� : {health}");
    }
}
