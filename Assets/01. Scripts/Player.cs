using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AudioClip deathClip;
    public float jumpForce = 0f;
    public float jumpCount = 0f;
    public float flips = 1f;

    private bool isRun = false;
    private bool isJump = true;
    private bool isDead = false;

    private Rigidbody2D playerRigidbody;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            return;
        }
        if (Input.GetKey(KeyCode.RightArrow) && jumpForce == 0)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(200f, 0));
            playerRigidbody.transform.eulerAngles = new Vector3(0,0);
            flips = 1f;
            isRun = true;
        }
        else if(Input.GetKeyUp(KeyCode.RightArrow) && jumpForce == 0)
        {
            playerRigidbody.velocity = Vector2.zero;
            isRun = false;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && jumpForce == 0)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(-200f, 0));
            playerRigidbody.transform.eulerAngles = new Vector3(0, 180);
            flips = -1f;
            isRun = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) && jumpForce == 0)
        {
            playerRigidbody.velocity = Vector2.zero;
            isRun = false;
        }
        animator.SetBool("isRun", isRun);

        if (Input.GetKey(KeyCode.Space) && jumpForce < 1700f && jumpCount == 0)
        {
            playerRigidbody.velocity = Vector2.zero;
            jumpForce += 850f * Time.deltaTime * 3;
        }
        if (Input.GetKeyUp(KeyCode.Space) && jumpCount == 0)
        {
            jumpCount++;
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(jumpForce*flips*0.6f, jumpForce));
        }
        animator.SetBool("isJump", isJump);
        
    }
    private void Die()
    {
        animator.SetTrigger("Die");
        playerRigidbody.velocity = Vector2.zero;
        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Dead" && !isDead)
        {
            Die();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.contacts[0].normal.y>0.7f)
        {
            isJump = false;
            jumpCount = 0;
            jumpForce = 0;
            playerRigidbody.velocity = Vector2.zero;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isJump = true;
    }
}
