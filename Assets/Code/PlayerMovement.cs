﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

enum States
{
    Default,
    Dashing,
}
public class PlayerMovement : MonoBehaviour // Script básico de movimentação para platformer 2D.
{
    [Header("Variáveis Gerais")]
    private States states;
    public Rigidbody2D playerRB;
    [SerializeField] private Transform groundContactPosition;
    [SerializeField] private LayerMask ground;
    private bool isOnGround;

    [Header("Config - Movimento Horizontal")]
    public float hrMovementSpeed = 10;
    private float hrInput;

    [Header("Config - Pulo")]
    [SerializeField] private float jumpHoldTimeConfig = 0.25f;
    private float jumpHoldTime; // Jogador pode segurar o pulo por esse tempo, possibilitando maior controle da altura.
    [SerializeField] private float jumpForce = 15;
    private bool isJumping;

    [Header("Config - Dash")]
    [SerializeField] private float dashSpeedMax;
    private float dashSpeed;
    private Vector2 dashDirection;
    private int hrInputCount;
    [SerializeField] private float timeBetweenPresses;
    private float dashInputTimer;

    // Métodos Default da Unity //
    private void Update()
    {
        switch (states) 
        {
            case States.Default:

                // Run Input //
                hrInput = Input.GetAxis("Horizontal");
                if (hrInput > 0)
                {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (hrInput < 0)
                {
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                }

                // Jump Input //

                isOnGround = Physics2D.OverlapCircle(new Vector2(this.transform.position.x, this.transform.position.y - 0.5f), 0.25f, ground);
                if (Input.GetKeyDown(KeyCode.Space) && isOnGround == true)
                {
                    isJumping = true;
                    jumpHoldTime = jumpHoldTimeConfig;
                }
                else if (Input.GetKeyUp(KeyCode.Space))
                {
                    jumpHoldTime = 0;
                }
                jumpHoldTime -= Time.deltaTime;
                if (jumpHoldTime <= 0)
                {
                    isJumping = false;
                }

                // Dash Input Omnidirecional //
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    dashDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position).normalized;
                    dashDirection.Normalize();
                    dashSpeed = dashSpeedMax;
                    states = States.Dashing;
                }

                // Dash Input Forward //
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    dashDirection = this.transform.right;
                    dashSpeed = dashSpeedMax;
                    states = States.Dashing;
                }

                // Dash Input Horizontal //
                dashInputTimer -= Time.deltaTime;
                if (dashInputTimer <= 0)
                {
                    hrInputCount = 0;
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (hrInputCount < 0)
                    {
                        hrInputCount = 0;
                    }
                    hrInputCount++;
                    dashInputTimer = timeBetweenPresses;
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (hrInputCount > 0)
                    {
                        hrInputCount = 0;
                    }
                    hrInputCount--;
                    dashInputTimer = timeBetweenPresses;
                }
                switch (hrInputCount)
                {
                    case 2:
                        hrInputCount = 0;
                        dashDirection = this.transform.right;
                        dashSpeed = dashSpeedMax;
                        states = States.Dashing;
                        break;
                    case -2:
                        hrInputCount = 0;
                        this.transform.rotation = Quaternion.Euler(0, 180, 0);
                        dashDirection = this.transform.right;
                        dashSpeed = dashSpeedMax;
                        states = States.Dashing;
                        break;
                }

                break;
            case States.Dashing:

                dashSpeed -= dashSpeed * 15 * Time.deltaTime;
                if (dashSpeed <= 1)
                {
                    states = States.Default;
                }

                break;
        }
    }
    private void FixedUpdate()
    {
        switch (states)
        {
            case States.Default:

                // Run //
                playerRB.velocity = new Vector2(hrInput * hrMovementSpeed, playerRB.velocity.y);

                // Jump //
                if (isJumping == true)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                }

                break;
            case States.Dashing:

                playerRB.velocity = dashDirection * dashSpeed;

                break;
        }
    }
}