using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager m_instance; // 싱글톤이 할당될 변수

    public static UIManager instance
    {
        get
        {
            if( m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>(); // 해당씬에 UIManager가 있는지 찾음
            }
            return m_instance;
        }
    }
    public Text ammoText; // 탄약 표시용 텍스트
    public Text scoreText; // 점수 표시용 텍스트
    public Text waveText; // 웨이브 표시용 텍스트
    public GameObject gameOverUI; // 게임 오버

    // 탄약 텍스트 갱신
    public void UpdateAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = magAmmo + " / " + remainAmmo; // 현재 탄창 총알 수와 남은 총알 수를 표시
    }
    // 점수 텍스트 갱신
    public void UpdateScoreText(int newScore)
    {
        scoreText.text = "Score: " + newScore; // 현재 점수를 표시
    }
    // 적 웨이브 텍스트 갱신
    public void UpdateWaveText(int waves, int count)
    {
        waveText.text = "Wave : " + waves + " \nEnemies Left : " + count; // 현재 웨이브와 적의 수를 표시
    }
    // 게임 오버 UI 표시
    public void SetActiveGameoverUI(bool active)
    {
        gameOverUI.SetActive(active); // 게임 오버 UI 활성화/비활성화
    }
    // 게임 재시작
    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬을 다시 로드하여 게임 재시작
    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
