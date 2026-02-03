using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProjectTileAddon : MonoBehaviour
{
    private Rigidbody rb;

    public int damage;
    
    private bool targetHit;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // private void OnTrrigerEnter(Collision Collision)
    // {
    //     if(targetHit)
    //         return;
    //     else
    //         targetHit = true;
    //     
    //     if (Collision.gameObject.GetComponent<Enemy>() != null)
    //     {
    //         Enemy enemy = Collision.gameObject.GetComponent<Enemy>();
    //         
    //         enemy.TakeDamage(damage);
    //         
    //         Destroy(gameObject);
    //     }
    //     
    //     rb.isKinematic = true;
    // }

    private void OnCollisionEnter(Collision collision)
    {
        //先にスフィアコライダーに当たって攻撃の判定がなくなっていた
        
        if(targetHit)
            return;
        else
            targetHit = true;
        
        Debug.Log("Hit"+collision.gameObject.name);
        
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Debug.Log("YesYesYes");
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            
            enemy.TakeDamage(damage);
            
            Destroy(gameObject);
            
            //弾が当たってもダメージが入らない
            //多分当たった判定すら出ていない
        }
        
        //rb.isKinematic = true;
    }
}
