/* 
 *  Author : Missnish
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    //--------------成员变量 public--------------
    public LayerMask plane;
    public float speed;
    public float jumpForce;
    [HideInInspector]
    public int score;
    public Text carrotNum;
    public AudioSource jumpMusic;
    public AudioSource HurtMusic;
    public AudioSource CarrotGetMusic;


    //--------------成员变量 private--------------
    bool action = false;
    Rigidbody2D rigidBody;
    Animator anim;
    BoxCollider2D collision;
    int jumpCount;
    bool isHurt;
    float moveDirection;

    ////--------------Unity主控函数--------------

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        collision = GetComponent<BoxCollider2D>();
        speed = 400;
        jumpForce = 300;
        score = 0;
        jumpCount = 0;
        moveDirection = 0;
        isHurt = false;
    }

    
    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
             action = true;
        }
    }
    private void FixedUpdate()
    {
        if(!isHurt)
        {
            Move();   
        }
        SwtichAnim();
        
    }

    //--------------自定义成员函数--------------
    void Move()
    {
        //移动
        //得到横向移动数据: -1-向左; 0-不动; 1-向右;
        moveDirection = Input.GetAxis("Horizontal");
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
        if(action && !anim.GetBool("IsDuck"))
        {
            switch(jumpCount > 1 ? false : true)
            {
                //跳起次数∈[1,2]
                case true :
                {
                    jumpMusic.Play();
                    //重构Player.y速度
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce * Time.deltaTime);
                    //动画切换 - 跳跃
                    anim.SetBool("IsJump", true);
                    jumpCount++;    
                }
                    break;
                //跳起次数超过2
                case false :
                    jumpCount++;                 
                    break;
                default :
                    break;
            }
            action = false;
        }

        //下蹲
        if(Input.GetKey(KeyCode.S))
        {
            anim.SetBool("IsDuck", true);
            collision.size = new Vector2(collision.size.x, 0.35f);
            collision.offset = new Vector2(collision.offset.x, -0.4f);
        }
        else
        {
            anim.SetBool("IsDuck", false);
            collision.size = new Vector2(collision.size.x, 0.85f);
            collision.offset = new Vector2(collision.offset.x, -0.16f);
        }
    }

    void SwtichAnim()
    {
        anim.SetBool("IsIdle", false);

        if(rigidBody.velocity.y < 0.1f && !collision.IsTouchingLayers(plane))
        {
            anim.SetBool("IsFall", true);
        }
        
        if(anim.GetBool("IsJump"))
        {
            if(rigidBody.velocity.y < 0)
            {
                anim.SetBool("IsJump", false);
                anim.SetBool("IsFall", true);
            }
        }
        else if(isHurt)
        {
            anim.SetBool("IsHurt", true); 
            anim.SetFloat("RunSpeed", 0f);
            if (Mathf.Abs(rigidBody.velocity.x) < 0.1f)
            {
                isHurt = false;
                anim.SetBool("IsHurt", false); 
                anim.SetBool("IsIdle", true);
            }
        }
        else if(collision.IsTouchingLayers(plane))
        {
            anim.SetBool("IsFall", false);
            anim.SetBool("IsIdle", true);
            jumpCount = 0;
        }
    }
    
    //收集物品
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Collection"))
        {
            CarrotGetMusic.Play();
            Destroy(other.gameObject);
            score++;
            carrotNum.text = score.ToString();
        }
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if(anim.GetBool("IsFall"))
            {
                enemy.JumpOn();
                //下落打击反馈 - 小跳跃
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce * Time.deltaTime);
                anim.SetBool("IsJump", true);
            }
            else
            {
                //平地打击反馈 - 左右弹飞
                isHurt = true;
                HurtMusic.Play();
                //判断player与collision的左右位置关系: 1-player在右; -1-player在左;
                int offsetX = transform.position.x - other.gameObject.transform.position.x > 0 ? 1 : -1;
                //判断player与collision的上下位置关系: 1-player在上; -1-player在下;
                int offsetY = transform.position.y - other.gameObject.transform.position.y > 0 ? 1 : -1;
                rigidBody.velocity = new Vector2(offsetX * 10, offsetY * 10);
                
            }
        }
        
    }

}
