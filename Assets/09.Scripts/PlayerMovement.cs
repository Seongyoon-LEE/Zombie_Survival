using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    Rigidbody rb;
    Animator animator;
    [SerializeField] float moveSpeed = 5f; // 이동속도
    [SerializeField] float rotSpeed = 180f; // 회전속도
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }


    void FixedUpdate() // 물리적인 갱신, 주기마다 움직임, 회전 애니메이션 처리 
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
