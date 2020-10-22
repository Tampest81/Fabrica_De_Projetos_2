using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
public class PlayerMovement : MonoBehaviour
{
    [Header("Variáveis Gerais")]
    public Rigidbody2D playerRB;
    [SerializeField] private LayerMask ground;

    [Header("Config - Movimento Horizontal")]
    public float hrMovementSpeed = 10;
    private float hrInput;

    [Header("Config - Pulo")]
    [SerializeField] private float jumpForce;
    private bool canJump;

    [Header("Config - Dash")]
    [SerializeField] private float dashSpeedMax;
    private float dashSpeed;
    private Vector2 dashDirection;
    private bool isDashing;
    private bool canDash;

    private bool knockback;
    private float knockbackTimer;
    private bool didKnockback;

    private void Update()
    {
        if (knockback)
        {
            Knockback();
            KnockbackTimer();
        }
        else
        {
            switch (isDashing)
            {
                case true:
                    DashTimer();
                    break;
                case false:
                    Jump();
                    WalkInput();
                    DashInput();
                    break;
            }
        }
    }
    private void FixedUpdate()
    {
        if (knockback)
        {

        }
        else
        {
            switch (isDashing)
            {
                case false:
                    Walk();
                    break;
                case true:
                    Dash();
                    break;
            }
        }
    }

    private void Jump()
    {
        if (Physics2D.OverlapCircle(new Vector2(this.transform.position.x, this.transform.position.y - 0.5f), 0.25f, ground))
        {
            canJump = true;
            canDash = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            canJump = false;
        }
        else if (Input.GetKeyUp(KeyCode.Space) && playerRB.velocity.y > 0)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y / 3);
        }
    }
    private void WalkInput()
    {
        hrInput = Input.GetAxis("Horizontal");
        if (hrInput > 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (hrInput < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void Walk()
    {
        if (hrInput != 0)
        {
            playerRB.velocity = new Vector2(hrInput * hrMovementSpeed, playerRB.velocity.y);
        }
    }
    private void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash)
        {
            dashDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
            dashDirection.Normalize();
            dashSpeed = dashSpeedMax;
            isDashing = true;
            canDash = false;
        }
    }
    private void Dash()
    {
        playerRB.velocity = dashDirection * dashSpeed;
    }
    private void DashTimer()
    {
        dashSpeed -= dashSpeed * 15 * Time.deltaTime;
        if (dashSpeed <= 20)
        {
            isDashing = false;
            playerRB.velocity /= 2;
        }
    }

    // PlaceHolder //
    [SerializeField] private float health;
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float amount, bool doesKnockback)
    {
        health -= amount;
        if (doesKnockback)
        {
            knockback = true;
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    private void Knockback()
    {
        if (!didKnockback)
        {
            playerRB.velocity = new Vector3(-1, 1, 0).normalized * 25;
            didKnockback = true;
        }
    }
    private void KnockbackTimer()
    {
        knockbackTimer -= Time.deltaTime;
        if (knockbackTimer <= 0)
        {
            knockback = false;
            didKnockback = false;
            knockbackTimer = 1;
        }
    }
}