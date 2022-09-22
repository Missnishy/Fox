/* 
 *  Author : Missnish
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class EnemyBee : Enemy
{
    //--------------成员变量 public--------------
    public float speed;

    //--------------成员变量 private--------------
    Rigidbody2D rigidBody;
    float upEdge;
    float downEdge;
    bool isfaceUp;     //Enemy面向flag: true-面向上(default); false-面向下;

    //--------------Unity主控函数--------------
    protected override void Start()
    {
        //获得父级组件
        base.Start();
        rigidBody = GetComponent<Rigidbody2D>();
        
        //变量初始化
        speed = 100;
        isfaceUp = true;   //  默认面向上

        //获取上下边界信息
        upEdge = GameObject.FindGameObjectWithTag("UpEdge").transform.position.y;
        downEdge = GameObject.FindGameObjectWithTag("DownEdge").transform.position.y;
        //获取完边界信息后销毁边界点
        Destroy(GameObject.FindGameObjectWithTag("UpEdge"));
        Destroy(GameObject.FindGameObjectWithTag("DownEdge"));

    }

    void FixedUpdate()
    {
        Move();
    }

    //--------------自定义成员函数--------------
    void Move()
    {
        if(isfaceUp)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, speed * Time.deltaTime);
            if((transform.position.y > upEdge))
            {
                isfaceUp = false;
            }
            
        }
        else
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -speed * Time.deltaTime);
            if((transform.position.y < downEdge))
            {
                isfaceUp = true;
            }
            
        }
    }



}
