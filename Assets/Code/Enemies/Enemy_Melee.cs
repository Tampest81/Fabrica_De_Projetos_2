using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class Enemy_Melee : Enemy
{
    private ContactFilter2D filter;
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_checkBoxPosition, checkBoxSize);

        Gizmos.DrawRay(this.transform.position + new Vector3(0, 1), playerDirection * aggroRange);
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
}