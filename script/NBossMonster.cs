using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class NBossMonster : MonoBehaviour
{
    private Transform playerTransform;
    public GameObject bulletPrefab;
    public GameObject player;
    public float fireRate = 2f; // Bullets per second
    private float nextFireTime = 0f;
    // Start is called before the first frame update
    private int BossHp = 1000;
    private int flag = 0;
    private PlayerController plc;
    public float minDelay = 1f; // Minimum delay between shots
    public float maxDelay = 3f; // Maximum delay between shots
    public int minBulletCount = 3; // Minimum number of bullets per shot
    public int maxBulletCount = 6; // Maximum number of bullets per shot
    private enum State
    {
        Idel
        //p1,
        //p2,
    }
    //private State _state;
    void Start()
    {
        bulletPrefab = GameObject.FindGameObjectWithTag("bullet");
        //_state=State.Idel;
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player object not found. Make sure the player is tagged as 'Player'.");
        }
        StartCoroutine(ShotBulletRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        /*if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
        }*/
        if (playerTransform != null && Time.time >= nextFireTime)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            //FireBullet();
            nextFireTime = Time.time + 1f / fireRate;
        }
        /*       switch(_state)
              {
                  case State.Idel:
                      if (playerTransform != null && Time.time >= nextFireTime)
                      {
                          Debug.Log("shot");
                          FireBullet();
                          nextFireTime = Time.time + 1f / fireRate;
                      }
                      break;
                 case State.p1:
                      break;
                  case State.p2;
                      break;
              }*/
    }
    void FireBullet()
    {
        //Debug.Log("shot");
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }
    // 에이전트에게 정보 넘겨주기 
    IEnumerator ShotBulletRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            ShotBullet();
        }
    }
    void ShotBullet()
    {
        int bulletCount = Random.Range(minBulletCount, maxBulletCount);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        for (int i = 0; i < bulletCount; i++)
        {
            //Vector2 direction = (playerTransform.position - transform.position).normalized;
            float angle = Random.Range(-10f, 10f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Vector2 ndirect = new Vector2(rotation.eulerAngles.x + direction.x, rotation.eulerAngles.y + direction.y);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(ndirect);
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //히트 설정 
        if (collision.gameObject.CompareTag("playerbullet"))
        {
            BossHp = BossHp - 10;
            /*if (BossHp < 991 && flag < 1)
            {
                plc.SetFlag(1);
            }
            if (BossHp < 750 &&flag<2)
            {
                flag = 2;
                plc.SetFlag(2);
            }
            if (BossHp < 500&&flag<3)
            {
                flag = 3;
                plc.SetFlag(3);
            }
            if (BossHp < 1)
            {
                plc.SetFlag(4);
            }*/
        }
    }
}
