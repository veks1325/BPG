using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerbullet : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 moveDirection;
    private Rigidbody2D rb;


    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;
    }
    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("BOSS"))
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BOSS"))
        {
            Destroy(gameObject);
        }
    }
}
