using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] internal float health;

    [Header("Movement Settings")]
    internal Rigidbody2D rb;

    [SerializeField] internal LayerMask groundLayer;
    [SerializeField] internal Vector3 checkBoxPosition;
    [SerializeField] internal Vector3 checkBoxSize;
    internal Vector3 _checkBoxPosition;

    [SerializeField] internal float walkSpeed;
    [SerializeField] internal float waitTime;
    internal float waitTimer;
    internal bool waiting;

    [Header("Attack Settings")]
    internal float aggroTimer;
    internal GameObject player;
    internal Vector3 playerDirection;
    internal bool aggroed = false;

    internal float playerDirAngle;

    [SerializeField] internal LayerMask playerMask;
    [SerializeField] internal float aggroRange;
    [SerializeField] internal float aggroTime;

    internal void Move()
    {
        rb.velocity = new Vector3(this.transform.right.x * walkSpeed, rb.velocity.y);
    }
    internal void WaitCheck()
    {
        if (!Physics2D.OverlapBox(_checkBoxPosition, checkBoxSize, 0, groundLayer))
        {
            waiting = true;
            waitTimer = waitTime;
        }
    }
    internal void Wait()
    {
        waitTimer -= Time.deltaTime;
        if (waitTimer <= 0)
        {
            waiting = false;
            Turn();
        }
    }
    private void Turn()
    {
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + Quaternion.Euler(0, 180, 0).eulerAngles);
        checkBoxPosition = -checkBoxPosition;
    }
    internal void LookForPlayer()
    {
        RaycastHit2D hit;
        if (Physics2D.Raycast(this.transform.position + new Vector3(0, 1), playerDirection, aggroRange, playerMask))
        {
            hit = Physics2D.Raycast(this.transform.position + new Vector3(0, 1), playerDirection, aggroRange, playerMask);
            if (hit.collider.tag == "Player")
            {
                if (playerDirAngle < 60)
                {
                    aggroed = true;
                    aggroTimer = aggroTime;
                }
            }
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    internal void AggroTimer()
    {
        aggroTimer -= Time.deltaTime;
        if (aggroTimer <= 0)
        {
            aggroed = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Ground")) && !waiting)
        {
            Turn();
        }    
    }
}