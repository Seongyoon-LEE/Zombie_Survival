using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager m_instance = null; // 싱글톤 인스턴스 
    // 싱글톤 접근용 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 앚기 오브젝트가 할당 되지 않았다면
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>(); // 현재 씬에서 GameManager를 찾음
            }
            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private int score = 0; // 현재 점수
    public bool isGameOver { get; private set; } = false; // 게임 오버 상태

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if(instance != this)
        {
            Destroy(gameObject); // 현재 오브젝트를 파괴
        }
    }
    private void Start()
    {
        // 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        FindObjectOfType<PlayerHealth>().onDeath += EndGame; // 매우 느림 씬에 있는 모든 게임 오브젝트를 순회 하면서 검색하기 때문 
    }
    // 점수를 추가하고 UI 갱신
    public void AddScore(int newScore)
    {
        // 게임 오버가 아닌 상태에서만 점수 증가 가능
        if(!isGameOver)
        {
            // 점수 추가
            score += newScore;
            // 점수 UI 텍스트 갱신
            UIManager.instance.UpdateScoreText(score);
        }
        
    }
    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameOver = true;
        // 게임 오버 UI를 활성화
        UIManager.instance.SetActiveGameoverUI(true);
    }

    void Update()
    {
        
    }
}
