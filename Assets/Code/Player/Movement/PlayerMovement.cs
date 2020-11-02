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

    // Gambiarra louca //
    [SerializeField] private bool isOnGround;
    private bool hasHitGround;
    private float jumpFallTimer;

    private bool knockback;
    private float knockbackTimer;
    private bool didKnockback;

    private WeaponManager weaponManager;

    private Animator animator;

    private void Start()
    {
        startingHealth = health;

        weaponManager = GetComponentInChildren<WeaponManager>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        jumpFallTimer -= Time.deltaTime;

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
        isOnGround = Physics2D.OverlapCircle(new Vector2(this.transform.position.x, this.transform.position.y - 0.5f), 0.25f, ground);
        if (isOnGround && !hasHitGround)
        {
            animator.Play("Jump Landing");
            jumpFallTimer = .3f;
            hasHitGround = true;
        }

        if (Physics2D.OverlapCircle(new Vector2(this.transform.position.x, this.transform.position.y - 0.5f), 0.25f, ground))
        {
            canJump = true;
            canDash = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            canJump = false;

            animator.Play("Jump");
            hasHitGround = false;
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

            if (isOnGround) animator.Play("Walk");
        }
        else if (hrInput < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            if (isOnGround) animator.Play("Walk");
        }
        else if (playerRB.velocity == Vector2.zero && jumpFallTimer <= 0)
        {
            animator.Play("Idle");
        }
    }
    private void Walk()
    {
        if (hrInput != 0)
        {
            playerRB.velocity = new Vector2(hrInput * hrMovementSpeed, playerRB.velocity.y);
        }
    }
    //
    private Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
    private void DashInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash)
        {
            dashDirection = (GetWorldPositionOnPlane(Input.mousePosition, 0) - this.transform.position).normalized;
            dashDirection.Normalize();
            dashSpeed = dashSpeedMax;
            isDashing = true;
            canDash = false;
        }
    }
    //
    private void Dash()
    {
        playerRB.velocity = dashDirection * dashSpeed;

        animator.Play("Dash");
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
    [SerializeField] public float health;
    private float startingHealth;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable_HP"))
        {
            float healAmount = startingHealth - health;
            if (healAmount < 5)
            {
                TakeDamage(-healAmount);
            }
            else
            {
                TakeDamage(-5);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Collectable_Ammo"))
        {
            Destroy(collision.gameObject);
            weaponManager.guns[weaponManager.currentGunIndex].CollectAmmo(20);
        }
    }
}