using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
public class WeaponManager : MonoBehaviour
{
    #region Variáveis
    public float ammoReservesTotal = 20;
    private float ammoReserves;
    public float magazineCurrent = 5;
    private float magazineMax = 5;
    public float reloadTime;
    public float reloadTimer;
    private Vector2 aimDirection;
    private float rotZ;
    private ContactFilter2D enemiesFilter;
    [SerializeField] private LayerMask enemiesLayer;
    [SerializeField] private GameObject bulletTrail;
    private List<RaycastHit2D> enemiesHit = new List<RaycastHit2D>();
    private States states;
    #endregion
    void Start()
    {
        enemiesFilter.useLayerMask = true;
        enemiesFilter.SetLayerMask(enemiesLayer);
    }
    void Update()
    {
        switch (states)
        {
            case States.Reloading:
                reloadTimer -= Time.deltaTime;
                if (reloadTimer <= 0)
                {
                    states = States.Default;
                }
                break;
            case States.Default:
                Shoot();
                if (Input.GetKeyDown(KeyCode.R) & ammoReservesTotal > 0)
                {
                    Reload();
                }
                break;
        }
        Aim();
    }
    private void Aim()
    {
        aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        aimDirection.Normalize();
        rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && magazineCurrent > 0)
        {
            enemiesHit.Clear();
            Physics2D.Raycast(this.transform.position, aimDirection, enemiesFilter, enemiesHit, 10);
            var trailInstance = Instantiate(bulletTrail, this.transform.position, Quaternion.Euler(0, 0, rotZ + -90));
            trailInstance.transform.localScale = new Vector3(.0625f, 10, 1);
            foreach (var element in enemiesHit)
            {
                element.collider.gameObject.GetComponent<TestEnemy>().Damage(1);
            }
            magazineCurrent--;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && ammoReservesTotal > 0)
        {
            Reload();
        }
    }
    private void Reload()
    {
        float reloadAmount;
        if (ammoReservesTotal > magazineMax)
        {
            reloadAmount = magazineMax - magazineCurrent;
        }
        else if (magazineCurrent == 0)
        {
            reloadAmount = ammoReservesTotal;
        }
        else
        {
            reloadAmount = ammoReservesTotal;
        }
        magazineCurrent += reloadAmount;
        ammoReservesTotal -= reloadAmount;
        reloadTimer = reloadTime;
        states = States.Reloading;
    }
}
