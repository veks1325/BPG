using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class PlayerController : Agent
{
    private GameObject bulletPrefab;
    private int PlayerHp;
    private Rigidbody2D rb;

    public float moveSpeed = 5f;
    public float jumpForce = 8.5f;

    private bool isGrounded = false;
    private bool isBase = false;

    private GameObject boss;
    public float fireRate = 2f;
    private float nextFireTime = 0f;

    public Camera agentCamera;

    void Start()
    {
        bulletPrefab = GameObject.FindGameObjectWithTag("playerbullet");
        PlayerHp = 100;
        rb = GetComponent<Rigidbody2D>();
        boss = GameObject.FindGameObjectWithTag("BOSS");

        if (agentCamera != null && agentCamera.gameObject.GetComponent<CameraSensorComponent>() == null)
        {
            var cameraSensor = agentCamera.gameObject.AddComponent<CameraSensorComponent>();
            cameraSensor.Camera = agentCamera;
            cameraSensor.SensorName = "AgentCamera";
            cameraSensor.Width = 84;
            cameraSensor.Height = 84;
            cameraSensor.Grayscale = false;
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void DownJump()
    {
        if (!isBase && isGrounded)
        {
            this.gameObject.layer = 6;
            isGrounded = false;
        }
        else if (isBase)
        {
            this.gameObject.layer = 6;
            Debug.Log("DownJump triggered");
            isGrounded = false;
            isBase = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isBase = false;
            this.gameObject.layer = 0;
        }
        else if (other.gameObject.CompareTag("base"))
        {
            isGrounded = true;
            isBase = true;
            this.gameObject.layer = 0;
        }
        else if (other.gameObject.CompareTag("bullet"))
        {
            PlayerHp -= 10;
            AddReward(-0.5f);
            if (PlayerHp < 1)
            {
                AddReward(-2f);
                EndEpisode();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    
    }
    void OnTriggerEnter2D(Collider2D other)
    {   
        if (other.gameObject.CompareTag("L2"))
        {
            PlayerHp = PlayerHp - 20;
            AddReward(-0.7f);
            if (PlayerHp < 1)
            {
                AddReward(-2f);
                EndEpisode();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (other.gameObject.CompareTag("thorn"))
        {
            PlayerHp = PlayerHp - 30;
            AddReward(-3f);
            if (PlayerHp < 1)
            {
                AddReward(-100f);
                EndEpisode();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void ClampPlayerPosition()
    {
        Vector3 pos = transform.position;
        Vector3 min = agentCamera.ViewportToWorldPoint(new Vector3(0, 0, agentCamera.nearClipPlane));
        Vector3 max = agentCamera.ViewportToWorldPoint(new Vector3(1, 1, agentCamera.nearClipPlane));

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        if (boss != null)
        {
            sensor.AddObservation(boss.transform.position);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(0.001f);
        float moveDirection = actions.ContinuousActions[0];
        int jumpInput = actions.DiscreteActions[0];
        int attackInput = actions.DiscreteActions[1];
        float fireX = actions.ContinuousActions[1];
        float fireY = actions.ContinuousActions[2];

        HMove(moveDirection);
        VMove(jumpInput);

        if (attackInput == 1)
        {
            //Fire();
            Fire(fireX, fireY);
        }
    }

    public void HMove(float moveDirection)
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    public void VMove(int input)
    {
        switch (input)
        {
            case 0: break;
            case 1: Jump(); break;
            case 2: DownJump(); break;
        }
    }
    //public void Fire()
    //{
    //    if (Time.time >= nextFireTime && boss != null)
    //    {
    //        // 보스 위치 가져오기
    //        Vector2 bossPosition = boss.transform.position;
    //        Vector2 direction = (bossPosition - (Vector2)transform.position).normalized;

    //        // 총알 생성 및 방향 설정

    //        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    //        playerbullet bulletScript = bullet.GetComponent<playerbullet>();
    //        if (bulletScript != null)
    //        {
    //            bulletScript.SetDirection(direction);
    //        }
    //        nextFireTime = Time.time + 1f / fireRate;
    //    }
    //}
    public void Fire(float x, float y)
    {
        if (Time.time >= nextFireTime)
        {
            Vector2 direction = new Vector2(x, y).normalized;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);
            }
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;
        var continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            discreteActions[0] = 2;
        }
        else
        {
            discreteActions[0] = 0;
        }
        
        discreteActions[1] = Input.GetKey(KeyCode.Q) ? 1 : 0;
    }
}
