using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponManager : MonoBehaviour
{
    private Vector2 aimDirection;
    private float rotZ;
    private ContactFilter2D enemiesFilter;
    [SerializeField] private LayerMask enemiesLayer;
    [SerializeField] private GameObject bulletTrail;
    private List<RaycastHit2D> enemiesHit = new List<RaycastHit2D>();

    void Start()
    {
        enemiesFilter.useLayerMask = true;
        enemiesFilter.SetLayerMask(enemiesLayer);
    }
    void Update()
    {
        aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        aimDirection.Normalize();
        rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            enemiesHit.Clear();
            Physics2D.Raycast(this.transform.position, aimDirection, enemiesFilter, enemiesHit, 10);
            var trailInstance = Instantiate(bulletTrail, this.transform.position, Quaternion.Euler(0, 0, rotZ + -90));
            trailInstance.transform.localScale = new Vector3(.0625f, 10, 1);

            foreach (var element in enemiesHit)
            {
                element.collider.gameObject.GetComponent<TestEnemy>().Damage(1);
            }
        }
    }
}
