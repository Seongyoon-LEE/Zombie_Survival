using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
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

    public GameObject playerPrefab; // 생성할 플레이어 캐릭터 프리팹

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
        //생성할 랜덤 위치 지정
        Vector3 randomSpawnPos = Random.insideUnitSphere * 5f; // 원점에서 반경 5유닛 내부의 랜덤 위치
        // 위치 y값은 0으로 변경
        randomSpawnPos.y = 0f; // y값을 0으로 설정

        // 네트워크 상의 모든 클라이언트들에서 생성 실행
        // 단 , 해당 게임 오브젝트의 주도권은, 생성 메서드를 직접 실행한 클라이언트들에게 있음
        PhotonNetwork.Instantiate(playerPrefab.name, randomSpawnPos, Quaternion.identity); // 플레이어 캐릭터 생성
        //// 플레이어 캐릭터의 사망 이벤트 발생시 게임 오버
        //FindObjectOfType<PlayerHealth>().onDeath += EndGame; // 매우 느림 씬에 있는 모든 게임 오브젝트를 순회 하면서 검색하기 때문 
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

    // 키보드 입력을 감지하고 룸을 나가게함
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // K 키를 누르면 룸을 나감
            PhotonNetwork.LeaveRoom();
        }
        
    }
    // 룸을 나간 후 실행되는 콜백
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene"); // 로비 씬으로 이동
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
   // 로컬 오브젝트라면 쓰기 부분이 실행됨
   if(stream.IsWriting)
        {
            // 네트워크를 통해 score 값을 보내기
            stream.SendNext(score);
        }
        else // 원격 오브젝트라면 읽기 부분이 실행됨
        {
            // 네트워크를 통해 score 값을 받기
            score = (int)stream.ReceiveNext();
            // UI 갱신
            UIManager.instance.UpdateScoreText(score);
        }
    }
}
