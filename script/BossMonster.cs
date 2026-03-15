using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class BossMonster : MonoBehaviour
{
    private Transform playerTransform;
    public GameObject bulletPrefab;
    public GameObject player;
    public GameObject L1prefab, L2prefab;

    public float fireRate = 2f;
    private float nextFireTime = 0f;
    private int BossHp = 1000;
    public float spawnInterval = 1f; // Time between spawns
    public float thornLifetime = 2f; // How long the thorns stay active
    public int thornCount = 20; // Number of thorns to spawn per cycle
    public int poolSize = 20; // Number of thorns in the pool
    public float groundWidth = 10f; // Width of the ground for spawning
    private bool isSpawning = true;
    private float currentXPosition; // Tracks the current spawn position along the ground
    private float stepSize;
    private PlayerController playerAgent;
    private GameObject ground;
    private GameObject thornPrefab;
    private enum State { Idle, Pattern1, Pattern2, Pattern3, Pattern4, Pattern5 }
    private State _state;
    //쿨 타임 
    private float p1c=0;
    private float p2c=7f;
    private float p3c=7f;
    private float p4c=15f;
    private float p5c=20f;
    //연산용
    private float nextp1 = 0;
    private float nextp2 = 0;
    private float nextp3 = 0;
    private float nextp4 = 0;
    private float nextp5 = 0;
    void Start()
    {
        _state = State.Pattern1;
        L1prefab = GameObject.Find("L1");
        L2prefab = GameObject.Find("L2");
        playerAgent = FindObjectOfType<PlayerController>();
        bulletPrefab = GameObject.FindGameObjectWithTag("bullet");
        player = GameObject.FindGameObjectWithTag("Player");
        thornPrefab = GameObject.FindGameObjectWithTag("thorn");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        ground = GameObject.FindGameObjectWithTag("Ground");
        if (ground != null)
        {
            groundWidth = ground.GetComponent<Renderer>().bounds.size.x;
        }
        stepSize = 0.6f;

        // Start from the leftmost position
        currentXPosition = -groundWidth / 2;

        // Start spawning coroutine
        //StartCoroutine(Thron());
        StartCoroutine(PatternExecutionCoroutine());
    }

    IEnumerator PatternExecutionCoroutine()
    {
        while (BossHp > 0)
        {
            switch (_state)
            {
                case State.Pattern1:
                    StartCoroutine(Pattern1Execution());
                    _state = State.Pattern2;
                    break;
                case State.Pattern2:
                    StartCoroutine(Pattern2Execution());
                    _state = State.Pattern3;
                    break;
                case State.Pattern3:
                    StartCoroutine(Pattern3Execution());
                    _state = State.Pattern4;
                    break;
                case State.Pattern4:
                    StartCoroutine(SpawnLaser());
                    _state = State.Pattern5;
                    break;
                case State.Pattern5:
                    StartCoroutine(Thron());
                    _state = State.Pattern1;
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator Pattern1Execution()
    {
        if (Time.time >= nextFireTime)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            FireBullet(direction);
            nextFireTime = Time.time + 1.7f / fireRate;
        }
        yield return null;
    }

    IEnumerator Pattern2Execution()
    {
        //p2 쿨타임 계산 
        if (Time.time >=nextp2){
            nextp2 = Time.time + p2c;
            for (int i = 0; i < 5; i++)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                Vector2 sinusoidalComponent = new Vector2(Mathf.Sin(i), Mathf.Cos(i));
                Vector2 finalDirection = (direction + sinusoidalComponent).normalized;
                FireBullet(finalDirection);
            }
            
        }
        yield return null;
    }

    IEnumerator Pattern3Execution()
    {
        if (Time.time >= nextp3)
        {
            nextp3 = Time.time + p3c;
            Vector2 startdirection = (playerTransform.position - transform.position).normalized;
            for (int i = 0; i < 16; i++)
            {
                Vector2 direction = Quaternion.Euler(0, 0, i * 22.5f) * startdirection;
                FireBullet(direction);
            }
            
        }
        yield return null;
    }

    IEnumerator SpawnLaser()
    {
        if (Time.time >= nextp4){
            nextp4 = p4c + Time.time;
            for (int i = 0; i < Random.Range(1, 4); i++)
            {
                float randomx = Random.Range(-10f, 10.0f);
                float randomy = Random.Range(20f, 21f);
                Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(135f, 225f));
                Vector3 laserSpawnPoint = new Vector3(randomx, randomy, 0f);
                GameObject laser = Instantiate(L1prefab, laserSpawnPoint, randomRotation);
                Destroy(laser, 0.2f);
                yield return new WaitForSeconds(1f);
                laser = Instantiate(L2prefab, laserSpawnPoint, randomRotation);
                Destroy(laser, 1f);
            }
            
        }
    }
    void FireBullet(Vector2 direction)
    {
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
        }
        else
        {
            Debug.LogError("Bullet prefab is null! Cannot fire.");
        }
    }
    IEnumerator Thron()
    {
        if (Time.time >= nextp5)
        {
            nextp5 = Time.time + p5c;
            // groundWidth 바닥 넓이 stepSize 스탭 사이즈
            int count = Mathf.FloorToInt(groundWidth / stepSize);
            for (int i = 0; i < count; i++)
            {
                // 스폰 위치 계산
                Vector2 spawnpoint = new Vector2(currentXPosition + (i * stepSize), ground.transform.position.y);

                // 프리팹 생성
                GameObject tron = Instantiate(thornPrefab, spawnpoint, Quaternion.identity);

                // 일정 시간 후에 삭제
                Destroy(tron, (count - i) * 0.1f+1f);

                // 다음 생성까지 대기
                yield return new WaitForSeconds(0.1f);
            }
            
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("playerbullet"))
        {
            if (playerAgent != null)
            {
                playerAgent.AddReward(1.0f); // 보스 맞춤 보상 추가
            }

            BossHp -= 10;
        }
    }
    public void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("playerbullet"))
        {
            if (playerAgent != null)
            {
                playerAgent.AddReward(1.0f); // 보스 맞춤 보상 추가
            }

            BossHp -= 10;
        }
    }
}