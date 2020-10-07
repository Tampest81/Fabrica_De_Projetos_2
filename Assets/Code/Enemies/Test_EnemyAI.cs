using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Test_EnemyAI : MonoBehaviour
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

    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;
    float playerDirAngle;

    private Gun attack;

    private float _attackTimeout;
    [SerializeField] private float _attackCount = 0;
    [SerializeField] private float attackCount = 3;
    private float _timer2nomeruimporra;

    private void Awake()
    {
        attack = new Gun(false, false, 0.5f, 0, 0.15f, 1, 0, 9999, 9999, 0, projectile, projectileSpeed, null);
        rb = this.GetComponent<Rigidbody2D>();
        _checkBoxPosition = this.transform.position + checkBoxPosition;
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }
    void Update()
    {
        _attackTimeout -= Time.deltaTime;
        _timer2nomeruimporra -= Time.deltaTime;

        _checkBoxPosition = new Vector3(this.transform.position.x + checkBoxPosition.x, this.transform.position.y);
        playerDirection = (player.transform.position + new Vector3(0, .5f)) - (this.transform.position + new Vector3(0, 1));
        playerDirection.Normalize();
        LookForPlayer();
        playerDirAngle = Vector3.Angle(this.transform.right, playerDirection);

        if (aggroed)
        {
            AggroTimer();
            Attack();
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
        if (aggroed)
        {

        }
        else
        {
            if (!waiting)
            {
                Move();
            }
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
    private void Attack()
    {
        if (_attackTimeout <= 0 && _attackCount < attackCount)
        {
            attack.Shoot(this.transform.position + new Vector3(0, 1), playerDirection, 0, false);
            _attackTimeout = .125f;
            _attackCount++;
        }
        else if (_timer2nomeruimporra <= 0)
        {
            _attackCount = 0;
            _timer2nomeruimporra = 3;
        }
    }
    
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