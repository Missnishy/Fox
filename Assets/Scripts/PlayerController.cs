using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    //--------------成员变量 public--------------
    public GameObject targetObj;
    public LayerMask plane;
    public float speed;
    public float jumpForce;
    [HideInInspector]
    public int score;

    //--------------成员变量 private--------------
    Rigidbody2D rigidBody;
    Animator anim;
    Collider2D collision;

    ////--------------Unity主控函数--------------

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        rigidBody = targetObj.GetComponent<Rigidbody2D>();
        anim = targetObj.GetComponent<Animator>();
        collision = targetObj.GetComponent<BoxCollider2D>();
        speed = 400;
        jumpForce = 300;
        score = 0;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        Move();
        SwtichAnimJumpToFall();
    }

    //--------------自定义成员函数--------------
    void Move()
    {
        //移动
        //得到横向移动数据: -1-向左; 0-不动; 1-向右;
        float moveDirection = Input.GetAxis("Horizontal");
        if (moveDirection != 0)
        {
            //重构Player.x速度 - 速度依附于组件RigidBody(刚体)
            rigidBody.velocity = new Vector2(moveDirection * speed * Time.deltaTime, rigidBody.velocity.y);
            //重构Player朝向 - Scale.x: -1-左; 1-右;
            transform.localScale = new Vector3((moveDirection > 0 ? 1 : -1), 1, 1);
            //动画切换 - 待机 & 跑动
            anim.SetFloat("RunSpeed", Mathf.Abs(moveDirection));
        }

        //跳跃
        if(Input.GetButton("Jump"))
        {
            //重构Player.y速度
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce * Time.deltaTime);
            //动画切换 - 跳跃
            anim.SetBool("IsJump", true);
        }
    }

    void SwtichAnimJumpToFall()
    {
        anim.SetBool("IsIdle", false);
        if(anim.GetBool("IsJump"))
        {
            if(rigidBody.velocity.y < 0)
            {
                anim.SetBool("IsJump", false);
                anim.SetBool("IsFall", true);
            }
        }
        else if(collision.IsTouchingLayers(plane))
        {
            anim.SetBool("IsFall", false);
            anim.SetBool("IsIdle", true);
        }
    }
    
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Collection"))
        {
            Destroy(other.gameObject);
            score++;
        }
    }
}
