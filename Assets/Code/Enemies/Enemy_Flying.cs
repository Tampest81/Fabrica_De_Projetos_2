using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Enemy_Flying : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private Vector3 playerPosition;
    private Vector3 playerDirection;
    private float playerDistance;

    private Rigidbody2D rb;

    private float rotZ;

    private float approachSpeed;
    private float retreatSpeed = 3;

    private float aggroRange = 15;
    private float chaseRange = 5;
    private bool aggroed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>().gameObject;
        
    }
    void Update()
    {
        FacePlayer();
        Movement();
    }

    private void Movement()
    {
        playerDistance = Vector3.Distance(this.transform.position, playerPosition);

        if (playerDistance <= aggroRange)
        {
            aggroed = true;
        }
        else
        {
            aggroed = false;
        }

        if (aggroed)
        {
            if (playerDirection.y > -0.42f)
            {
                print("a");
                rb.AddForce(Vector3.up * retreatSpeed);
            }

            if (playerDistance > chaseRange)
            {
                approachSpeed = playerDistance;

                rb.AddForce(playerDirection * approachSpeed);
            }
            else
            {
                rb.AddForce(-playerDirection * retreatSpeed);
            }
        }
    }
    private void FacePlayer()
    {
        playerPosition = player.transform.position;
        playerDirection = playerPosition - this.transform.position;
        playerDirection.Normalize();
        rotZ = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;

        this.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        if (this.transform.rotation.eulerAngles.z > 90 && this.transform.rotation.eulerAngles.z < 270)
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(this.transform.position, playerDirection * aggroRange);
    }
}