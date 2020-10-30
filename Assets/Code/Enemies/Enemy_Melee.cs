using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class Enemy_Melee : MonoBehaviour
{
    [SerializeField] private float health;

    Rigidbody2D rb;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Vector3 checkBoxPosition;
    [SerializeField] Vector3 checkBoxSize;
    Vector3 _checkBoxPosition;
    
    [SerializeField] float walkSpeed;
    [SerializeField] float waitTime;
    float waitTimer;
    bool waiting;

    [SerializeField] LayerMask playerMask;
    [SerializeField] float aggroRange;
    [SerializeField] float aggroTime;
    float aggroTimer;
    GameObject player;
    Vector3 playerDirection;
    bool aggroed = false;

    private ContactFilter2D filter;

    float playerDirAngle;

    private float _attackTimeout;
    private bool attacking;
    [SerializeField] private Collider2D attackHitbox;

    private void Awake()
    {
        filter.layerMask = playerMask;

        rb = this.GetComponent<Rigidbody2D>();
        _checkBoxPosition = this.transform.position + checkBoxPosition;
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }
    void Update()
    {
        _attackTimeout -= Time.deltaTime;

        _checkBoxPosition = new Vector3(this.transform.position.x + checkBoxPosition.x, this.transform.position.y);
        if(player) playerDirection = (player.transform.position + new Vector3(0, .5f)) - (this.transform.position + new Vector3(0, 1));
        playerDirection.Normalize();
        LookForPlayer();
        playerDirAngle = Vector3.Angle(this.transform.right, playerDirection);

        if (aggroed)
        {
            AggroTimer();
            //Attack();
            if (!attacking)
            {
                StartCoroutine("Attack");
            }
        }
        else
        {
            if (waiting)
            {
                Wait();
            }
            else
            {
                WaitCheck();
            }
        }
    }
    private void FixedUpdate()
    {
        if (!aggroed && !waiting && !attacking)
        {
            Move();
        }
    }
    private void OnDrawGizmos() // Visualização dos raycasts;
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_checkBoxPosition, checkBoxSize);

        Gizmos.DrawRay(this.transform.position + new Vector3(0, 1), playerDirection * aggroRange);
    }

    private void Move()
    {
        rb.velocity = new Vector3(this.transform.right.x * walkSpeed, rb.velocity.y);
    }
    private void WaitCheck()
    {
        if (!Physics2D.OverlapBox(_checkBoxPosition, checkBoxSize, 0, groundLayer))
        {
            waiting = true;
            waitTimer = waitTime;
        }
    }
    private void Wait()
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
    private void LookForPlayer()
    {
        RaycastHit2D hit;
        if(Physics2D.Raycast(this.transform.position + new Vector3(0, 1), playerDirection, aggroRange, playerMask))
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
    private void AggroTimer()
    {
        aggroTimer -= Time.deltaTime;
        if (aggroTimer <= 0)
        {
            aggroed = false;
        }
    }

    IEnumerator Attack()
    {
        List<Collider2D> hit = new List<Collider2D>();
        if (_attackTimeout <= 0)
        {
            attacking = true;
            rb.AddForce((playerDirection + Vector3.up).normalized * 1000);
            _attackTimeout = 2;
            yield return new WaitForSeconds(.25f);
            Physics2D.OverlapCollider(attackHitbox, filter, hit);
            foreach (var element in hit)
            {
                if(element.GetComponent<PlayerMovement>())
                    element.GetComponent<PlayerMovement>().TakeDamage(5, true);
            }
            yield return new WaitForSeconds(.125f);
            rb.AddForce(-playerDirection.normalized * 1000);
            attacking = false;
        }
    }

    //private void Attack()
    //{
    //    if (_attackTimeout <= 0)
    //    {
    //        jumpBack = false;
    //        attacking = true;
    //        rb.AddForce((playerDirection + Vector3.up).normalized * 1000);
    //        _attackTimeout = 2;
    //    }
    //    else
    //    {
    //        if (!jumpBack)
    //        {
    //            rb.AddForce(-playerDirection.normalized * 1000);
    //            jumpBack = true;
    //        }
    //        attacking = false;
    //    }
    //}

    // PlaceHolder //
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}