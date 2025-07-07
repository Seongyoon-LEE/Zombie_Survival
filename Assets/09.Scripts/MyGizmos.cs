using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color color = Color.red; // Gizmos 색상
    public float radius = 0.5f; // Gizmos 반지름
    private void OnDrawGizmos()
    {
        // Gizmos 색상 설정
        Gizmos.color = color;
        // 현재 오브젝트의 위치에 구체를 그리기
        Gizmos.DrawSphere(transform.position, radius);
    }
}
