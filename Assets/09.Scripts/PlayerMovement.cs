using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    Rigidbody rb;
    Animator animator;
    [SerializeField] float moveSpeed = 5f; // �̵��ӵ�
    [SerializeField] float rotSpeed = 180f; // ȸ���ӵ�
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    void FixedUpdate() // �������� ����, �ֱ⸶�� ������, ȸ�� �ִϸ��̼� ó�� 
    {
        Move();
        Rotate();
    }
    private void Move()
    {
        Vector3 moveDistance = 
        playerInput.move * transform.forward * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDistance);    
    }
    private void Rotate()
        {
            float turn = playerInput.rotate * rotSpeed * Time.fixedDeltaTime;
        rb.rotation = rb.rotation * Quaternion.Euler(0f, turn, 0f);
        }
}
