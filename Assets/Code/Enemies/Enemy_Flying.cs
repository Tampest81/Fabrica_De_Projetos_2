using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
public class Enemy_Flying : MonoBehaviour
{
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackCooldown;
    private float _attackDuration;
    private float _attackCooldown;
    private bool attacking = false;
    [SerializeField] private float damage;
    [SerializeField] private float damageRate;
    private float _damageRate;

    private Vector3 playerDir;
    private float rotZ;
    [SerializeField] private float turnSpeed;
    private float _turnSpeed;

    private LineRenderer lineRenderer;
    private RaycastHit2D hit;
    private float range = 100;

    [SerializeField] private float attackChargeUpDuration;
    private float _attackChargeUpDuration;

    private bool decidedDirection = false;
    private string turnDirection;

    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private Vector3 playerPosition;
    private Vector3 playerDirection;
    private float playerDistance;

    private Rigidbody2D rb;

    private float approachSpeed;
    private float retreatSpeed = 3;

    private float aggroRange = 15;
    private float chaseRange = 5;
    private bool aggroed = false;

    [SerializeField] private float health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>().gameObject;

        _attackCooldown = attackCooldown;
        _attackDuration = attackDuration;

        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
    void Update()
    {
        _damageRate -= Time.deltaTime;
        if (attacking)
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotZ));

            if (_attackDuration <= 0)
            {
                attacking = false;
                _attackDuration = attackDuration;
            }

            if (_attackChargeUpDuration <= 0)
            {
                _turnSpeed += turnSpeed;
                Raycast();
                LaserPos();
                lineRenderer.enabled = true;
                _attackDuration -= Time.deltaTime;
                spriteRenderer.color = Color.red;

                if (hit.collider && hit.collider.CompareTag("Player") && _damageRate <= 0)
                {
                    player.GetComponent<PlayerMovement>().TakeDamage(damage);
                    _damageRate = damageRate;
                }

                if (player && player.transform.position.x < this.transform.position.x && !decidedDirection)
                {
                    decidedDirection = true;
                    turnDirection = "left";
                }
                else if (!decidedDirection)
                {
                    decidedDirection = true;
                    turnDirection = "right";
                }

                if (turnDirection == "right")
                {
                    rotZ = rotZ + 1 * Time.deltaTime * _turnSpeed;
                }
                else
                {
                    rotZ = rotZ - 1 * Time.deltaTime * _turnSpeed;
                }
            }
            else
            {
                _attackChargeUpDuration -= Time.deltaTime;
                spriteRenderer.color = Color.yellow;

                FacePlayer();
            }
        }
        else
        {
            decidedDirection = false;

            _attackChargeUpDuration = attackChargeUpDuration;
            _turnSpeed = 0;

            lineRenderer.enabled = false;

            FacePlayer();

            _attackCooldown -= Time.deltaTime;

            if (aggroed && _attackCooldown <= 0)
            {
                attacking = true;
                _attackCooldown = attackCooldown;
            }

            // Temporary //
            spriteRenderer.color = Color.green;
        }
        Movement();
    }
    private void Movement()
    {
        playerDistance = Vector3.Distance(this.transform.position, playerPosition);

        if (player && playerDistance <= aggroRange)
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
        if (player)
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
    }

    private void Raycast()
    {
        hit = Physics2D.Raycast(this.transform.position, this.transform.right, range);
    }
    private void LaserPos()
    {
        lineRenderer.SetPosition(0, this.transform.position);

        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, transform.right * range);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(this.transform.position, playerDirection * aggroRange);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}