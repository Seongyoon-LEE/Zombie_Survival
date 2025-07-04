using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Input Axis Names")]
    public string moveAxisName = "Vertical"; // �յ� �̵� �Է�
    public string rotateAxisName = "Horizontal"; // �¿� ȸ�� �Է�
    public string fireButtonName = "Fire1"; // ���� �Է�
    public string reloadButtonName = "Reload"; // ������ �Է�

    // ������Ƽ �Է°� ����
    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }

    void Update()
    {
        if(GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            move = 0f; rotate = 0f; fire = false; reload = false;
            return; // ���� ���� ���¸� �Է� ���� ����
        }
        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButton(reloadButtonName);
    }
}
