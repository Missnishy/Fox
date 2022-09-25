/* 
 *  Author : Missnish
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ShowDialog : MonoBehaviour
{
    //--------------成员变量 public--------------
    public GameObject UI;


    //--------------成员变量 private--------------


    //--------------Unity主控函数--------------
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //--------------自定义成员函数--------------
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            UI.SetActive(true);
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            UI.SetActive(false);
        }
    }

}
