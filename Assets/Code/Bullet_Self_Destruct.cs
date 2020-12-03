using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Self_Destruct : MonoBehaviour
{
    private WeaponManager weaponManager;
    private Vector3 startPos;
    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
        startPos = this.transform.position;
    }
    private void Update()
    {
        if (Vector3.Distance(startPos, this.transform.position) > weaponManager.guns[weaponManager.currentGunIndex]._range * 2)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
