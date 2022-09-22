/* 
 *  Author : Missnish
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class EnemySlug : Enemy
{
    //--------------成员变量 public--------------
    public float speed;

    //--------------成员变量 private--------------
    Rigidbody2D rigidBody;
    float leftEdge;
    float rightEdge;
    bool isfaceLeft;     //Enemy面向flag: true-面向左(default); false-面向右;

    //--------------Unity主控函数--------------
    protected override void Start()
    {
        //获得父级组件
        base.Start();
        rigidBody = GetComponent<Rigidbody2D>();
        
        //获取左右边界信息
        leftEdge = GameObject.FindGameObjectWithTag("LeftEdge").transform.position.x;
        rightEdge = GameObject.FindGameObjectWithTag("RightEdge").transform.position.x;
        //获取完边界信息后销毁边界点
        Destroy(GameObject.FindGameObjectWithTag("LeftEdge"));
        Destroy(GameObject.FindGameObjectWithTag("RightEdge"));

        speed = 100;
        isfaceLeft = true;   //  默认面向左
    }

    
    private void FixedUpdate()
    {
        Move();
    }


    //--------------自定义成员函数--------------
    void Move()
    {
        if(isfaceLeft)
        {
            rigidBody.velocity = new Vector2(-speed * Time.deltaTime, rigidBody.velocity.y);
            if((transform.position.x < leftEdge))
            {
                isfaceLeft = false;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            
        }
        else
        {
            rigidBody.velocity = new Vector2(speed * Time.deltaTime, rigidBody.velocity.y);
            if((transform.position.x > rightEdge))
            {
                isfaceLeft = true;
                transform.localScale = new Vector3(1, 1, 1);
            }
            
        }
        
    }
    
}
