using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 유니티 전용 포톤 컴포넌트
using Photon.Realtime; // 포톤 서비스 관련 라이브러리 
using UnityEngine.UI; // UI 관련 라이브러리

// 매치메이킹(마스터) 서버와 룸 접속 담당 
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1"; // 게임 버전
    public Text connectInfoTxt; // 네트워크 정보 표시할 텍스트 
    public Button joinBtn; // 룸 접속 버튼                            

    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion; // 게임 버전 설정
        PhotonNetwork.ConnectUsingSettings(); // 포톤 네트워크로 버전 별로 접속 
        joinBtn.interactable = false; // 룸 접속 버튼 비활성화
        connectInfoTxt.text = "Connecting to Master server..."; // 접속중 메시지 표시
    }
    // 마스터 서버 접속 성공시 자동실행
    public override void OnConnectedToMaster()
    {
        joinBtn.interactable = true; // 룸 접속 버튼 활성화
        connectInfoTxt.text = "온라인 : 마스터 서버와 연결됨..."; // 접속 성공 메시지 표시
    }
    // 마스터 서버 접속 실패시 자동 실행 
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinBtn.interactable = false; // 룸 접속 버튼 활성화
        connectInfoTxt.text = "오프라인 : 마스터 서버와 연결 끊김\n 접속 재시도..."; // 접속 성공 메시지 표시
    }
    public void Connect()
    {
        joinBtn.interactable = false; // 중복 접속을 막기 위해 접속 버튼을 비활성화 
        if(PhotonNetwork.IsConnected) // 마스터 접속중이라면
        {
            connectInfoTxt.text = "룸에 접속..."; // 룸 접속 메시지 표시
            PhotonNetwork.JoinRandomRoom(); // 빈 룸에 참가 시도
        }
        else // 마스터 서버 접속중이 아니라면
        {
            connectInfoTxt.text = "오프라인 : 마스터 서버와 연결 되지 않음\n 접속 재시도..."; // 마스터 서버 접속 메시지 표시        
            PhotonNetwork.ConnectUsingSettings(); // 마스터 서버 접속 시도
        }
    }
    // 빈방이 없어 랜덤 룸 참가에 실패한경우 
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectInfoTxt.text = "빈 룸이 없음 새로운 방 생성..."; // 룸 접속 메시지 표시
        // 최대 4명까지 접속 가능한 빈방 생성 
        PhotonNetwork.CreateRoom(null, new RoomOptions {IsOpen = true, IsVisible = true , MaxPlayers = 4 }, TypedLobby.Default); // 빈 방 생성
    }
    // 룸에 참가된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        connectInfoTxt.text = "룸 접속 성공!"; // 룸 접속 성공 메시지 표시
        // 모든 참가자가 메인씬을 로드하게 하였음 
        PhotonNetwork.LoadLevel("MainScene"); // 게임 씬으로 전환
        // 위의 메서드가 실행 되면 다른 플레이어들의 컴퓨터에서도 자동으로 
        // PhotonNetwork.LoadLevel("MainScene"); 이 실행되어 방장과 같은 씬을 로드하게 된다.  
        // PhotonNetwork.LoadLevel를 하면 좋은점은 뒤늦게 해당 룸에 입장한 다른 플레이어가
        // PhotonNetwork.LoadLevel로 기존 플레이어들과 같은 씬에 도착했을때 도중에 참가한
        // 플레이어도 해당 씬의 모습이 다른 플레이어도 해당 씬의 모습이 다른 플레이어 보는
        // 씬의 모습과 동일하게 자동 구성 되어 굉장히 편하다.
    }
}
