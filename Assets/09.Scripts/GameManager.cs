using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null; // �̱��� �ν��Ͻ� 
    public bool isGameOver = false; // ���� ���� ���� 

    void Awake()
    {
        if (Instance == null)
            Instance = this; // �̱��� �ν��Ͻ� ���� 
        else if (Instance != this)
            Destroy(gameObject); // �ٸ� �ν��Ͻ��� �����ϸ� �ı� 
    }


    void Update()
    {
        
    }
}
