using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponManager : MonoBehaviour
{
    void Start()
    {

    }
    void Update()
    {
        Aim();
        Shoot();
    }

    // Mira //
    private Vector2 aimDirection;
    private float rotZ;
    private void Aim()
    {
        aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    // Tiros //
    [Header("Configs - Tiros")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletVelocity;
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var tmpBullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
            tmpBullet.GetComponent<Rigidbody2D>().velocity = aimDirection.normalized * bulletVelocity;
            Destroy(tmpBullet, .5f);
        }
    }
}
