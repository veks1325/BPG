using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oldstylectrl : MonoBehaviour
{
    private int PlayerHp;
    private Rigidbody2D rb;
    public float moveSpeed = 7f;
    public float jumpForce = 10f;

    private bool isGrounded = false;
    private bool isBase = false;

    private Camera mainCamera;


    void Start()
    {
        PlayerHp = 1000;
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        float moveDirection = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);

        if (moveDirection > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveDirection < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        ClampPlayerPosition();
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
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
        else if (isBase&&(Input.GetKeyDown(KeyCode.DownArrow)))
        {
            
            this.gameObject.layer = 6;
            Debug.Log("down");
            Debug.Log(this.gameObject.layer);
            isGrounded = false;
            isBase = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            //땅과 닿으면  그라운드 
            isGrounded = true;
            isBase = false;
            this.gameObject.layer = 0;
        }
        if (other.gameObject.CompareTag("base"))
        {
            isGrounded = true;
            isBase = true;
            this.gameObject.layer = 0;
        }
        if (other.gameObject.CompareTag("L2"))
        {
            PlayerHp = PlayerHp - 10;
            if (PlayerHp < 1)
            {
                //ExitGame();
            }
        }
        if (other.gameObject.CompareTag("bullet"))
        {
            PlayerHp =  PlayerHp - 10;
            if (PlayerHp < 1)
            {
                //ExitGame();
            }
        }
    }
    //void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.CompareTag("base"))
    //    {
    //        isGrounded = false;
    //        isBase = false;
    //        this.gameObject.layer = 0;
    //    }
    //}
    //카메라에서 벗어나지 않게 
    void ClampPlayerPosition()
    {
        Vector3 pos = transform.position;
        Vector3 min = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 max = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // 어플리케이션 종료
#endif
    }

}