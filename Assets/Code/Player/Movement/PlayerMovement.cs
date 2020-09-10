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

    private void Update()
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
    private void FixedUpdate()
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
        else if (Input.GetKeyUp(KeyCode.Space))
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
}