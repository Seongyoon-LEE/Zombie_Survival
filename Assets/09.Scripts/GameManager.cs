using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null; // 싱글톤 인스턴스 
    public bool isGameOver = false; // 게임 오버 상태 

    void Awake()
    {
        if (Instance == null)
            Instance = this; // 싱글톤 인스턴스 생성 
        else if (Instance != this)
            Destroy(gameObject); // 다른 인스턴스가 존재하면 파괴 
    }


    void Update()
    {
        
    }
}
