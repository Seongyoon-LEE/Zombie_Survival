using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Zombie zombiePrefab; // 좀비 프리팹

    public ZombieData[] zombieDatas; // 좀비 데이터 배열
    public List<Transform> spawnPointList; // 좀비가 생성될 위치 배열

    private List<Zombie> zombieList = new List<Zombie>(); // 생성된 좀비 리스트
    private int wave; // 현재 웨이브

    private void Start()
    {
        var spawnPoint = GameObject.Find("Spawn Points"); // "SpawnPoints" 오브젝트를 찾아서
        if(spawnPoint != null)
        {
            spawnPoint.GetComponentsInChildren<Transform>(spawnPointList); // 자식 오브젝트들을 spawnPointList에 저장
        }
        spawnPointList.RemoveAt(0); // 첫 번째 요소는 부모 오브젝트이므로 제거

        zombieDatas = Resources.LoadAll<ZombieData>("ZombieDatas"); // Resources 폴더에서 ZombieData를 로드
    }

    void Update()
    {
        // 게임 오버 상태일때는 생성하지 않음
        if(GameManager.instance != null && GameManager.instance.isGameOver)
        {
            return;
        }
        // 좀비를 모두 물리친 경우 다음 스폰 실행
        if (zombieList.Count <= 0)
        {
            SpawnWave();
        }
        // UI 갱신
        UpdateUI();
    }
    private void UpdateUI()
    {
        // 현재 웨이브와 남은 적 수 표시
        UIManager.instance.UpdateWaveText(wave,zombieList.Count);
        
    }
    // 현재 웨이브에 맞춰 좀비들을 생성
    private void SpawnWave()
    {
        wave++; // 웨이브 증가
        int spawnCount = Mathf.RoundToInt(wave * 1.5f); // 웨이브에 따라 생성할 좀비 수 결정
        for(int i = 0; i < spawnCount; i++)
        {
            CreateZomvie();
        }
    }
    // 좀비를 생성하고 생성한 좀비에게 추적할 대상을 할당
    private void CreateZomvie()
    {
        ZombieData zombieData = zombieDatas[Random.Range(0, zombieDatas.Length)]; // 랜덤으로 좀비 데이터 선택

        Transform spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Count)]; // 랜덤으로 스폰 위치 선택
        
        Zombie zombie = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation); // 좀비 생성
        zombie.Setup(zombieData); // 좀비 데이터 AI 스펙을 설정

        zombieList.Add(zombie); // 생성한 좀비를 리스트에 추가
        // 람다식으로 익명의 메서드를 만듬
        zombie.onDeath += () => zombieList.Remove(zombie); // 좀비가 죽으면 리스트에서 제거
        // 람다식으로 익명의 메서드를 만듬
        zombie.onDeath += () => Destroy(zombie.gameObject, 5f); // 좀비가 죽으면 5초 후에 제거
        zombie.onDeath += () => GameManager.instance.AddScore(100); // 좀비가 죽으면 점수 추가
    }
}
