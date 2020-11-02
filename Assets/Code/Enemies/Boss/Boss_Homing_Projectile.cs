using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Homing_Projectile : MonoBehaviour
{
    [SerializeField] private float spriteRotSpeed;
    [SerializeField] private GameObject sprite;

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float homingStrength;

    [SerializeField] private float damage;
    [SerializeField] private float damageTickRate;
    private float _damageTickRate;

    private GameObject player;
    private Vector3 playerDir;

    private Rigidbody2D rb;

    void Start()
    {
        sprite = Instantiate(sprite, this.transform.position, Quaternion.identity);
        player = GameObject.FindWithTag("Player");
        rb = this.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        playerDir = player.transform.position - this.transform.position;
        playerDir.Normalize();

        _damageTickRate -= Time.deltaTime;

        sprite.transform.position = this.transform.position;
        sprite.transform.Rotate(new Vector3(0, 0, 1), spriteRotSpeed);
    }
    void FixedUpdate()
    {   
        rb.velocity = transform.up * projectileSpeed ;
        float rotatingIndex = Vector3.Cross(playerDir, transform.up).z;
        rb.angularVelocity = -1 * rotatingIndex * homingStrength;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_damageTickRate <= 0)
            {
                collision.gameObject.GetComponent<PlayerMovement>().TakeDamage(damage);
                _damageTickRate = damageTickRate;
            }
        }
    }
    private void OnDestroy()
    {
        Destroy(sprite);
    }
}