using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Minion_Homing : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float homingStrength;

    [SerializeField] private float damage;

    private GameObject player;
    private Vector3 playerDir;

    private Rigidbody2D rb;

    private float timer = 1;

    private float rotZ;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = this.GetComponent<Rigidbody2D>();
    }
    void Update()
    {   
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            this.transform.rotation = Quaternion.Euler(0, 0, rotZ - 90);
        }
        playerDir = player.transform.position - this.transform.position;
        playerDir.Normalize();
        rotZ = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg;
    }
    void FixedUpdate()
    {   
        if(timer <= 0)
        {
            rb.velocity = transform.up * projectileSpeed ;
            float rotatingIndex = Vector3.Cross(playerDir, transform.up).z;
            rb.angularVelocity = -1 * rotatingIndex * homingStrength;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage()
    {
        Destroy(this.gameObject);
    }
}