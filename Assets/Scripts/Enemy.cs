/* 
 *  Author : Missnish
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Enemy : MonoBehaviour
{

    //--------------成员变量 protected--------------
    protected Animator anim;
    protected AudioSource deathMusic;

    //--------------Unity主控函数--------------
    protected virtual void Start()
    {
        //组件初始化
        anim = GetComponent<Animator>();
        deathMusic = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    //--------------自定义成员函数--------------
    public void JumpOn()
    {
        anim.SetTrigger("IsDeath");
        deathMusic.Play();
    }
    public void Death()
    {
        Destroy(gameObject);
    }

}
