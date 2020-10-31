using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Sentry : Enemy
{
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;

    private Gun attack;

    private float _attackTimeout;
    private float _attackCount = 0;
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
        if (player) playerDirection = (player.transform.position + new Vector3(0, .5f)) - (this.transform.position + new Vector3(0, 1));
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
        if (!aggroed)
        {
            if (!waiting)
            {
                Move();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_checkBoxPosition, checkBoxSize);

        Gizmos.DrawRay(this.transform.position + new Vector3(0, 1), playerDirection * aggroRange);
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
}