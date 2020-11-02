using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform shootOrigin;

    [SerializeField] private GameObject[] tmpArr;
    [SerializeField] public static GameObject[] muzzleFlash;
    public static float rotZ;


    public static Vector2 aimDirection;

    [SerializeField] Sprite pistolSprite;
    [SerializeField] Sprite shotgunSprite;
    [SerializeField] Sprite machinegunSprite;

    public Gun[] guns = new Gun[3];
    private Gun pistol = new Gun(true, false, 0.3f, 1, 0, 1, 10, 20, 5, 0.5f, null, 0, null);
    private Gun shotgun = new Gun(true, false, 0.3f, 1, 0.2f, 10, 7.5f, 10, 2, 1, null, 0, null);
    private Gun machinegun = new Gun(true, true, 0.05f, 1, 0.1f, 1, 10, 200, 50, 2, null, 0, null);
    public int currentGunIndex;

    SpriteRenderer spriteRenderer;

    RaycastHit2D[] raycastResults = new RaycastHit2D[1];

    [SerializeField] private LayerMask layersToHit;

    private void Awake()
    {
        muzzleFlash = tmpArr;

        guns[0] = pistol;
        guns[1] = shotgun;
        guns[2] = machinegun;

        currentGunIndex = 0;

        pistol.gunSprite = pistolSprite;
        shotgun.gunSprite = shotgunSprite;
        machinegun.gunSprite = machinegunSprite;

        pistol.ReloadOnAwake();
        shotgun.ReloadOnAwake();
        machinegun.ReloadOnAwake();

        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        switch (guns[currentGunIndex]._isReloading)
        {
            case true:

                guns[currentGunIndex]._reloadTimer -= Time.deltaTime;
                if (guns[currentGunIndex]._reloadTimer <= 0)
                    guns[currentGunIndex]._isReloading = false;

                break;
            case false:

                SelectGun();
                guns[currentGunIndex].Shoot(shootOrigin.position, aimDirection, layersToHit, true);
                guns[currentGunIndex].Reload();

                break;
        }
        //Aim();
        AimPerspective();
        UpdateGunSprite();

        Debug.DrawRay(this.transform.position, aimDirection*10, Color.red);
    }

    private void Aim()
    {
        aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - shootOrigin.position;
        aimDirection.Normalize();
        rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotZ);
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
    private void AimPerspective()
    {
        aimDirection = GetWorldPositionOnPlane(Input.mousePosition, 0) - shootOrigin.position;
        aimDirection.Normalize();
        rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    //
    private void SelectGun()
    {
        currentGunIndex += (int)Input.mouseScrollDelta.y;
        if (currentGunIndex > 2)
            currentGunIndex = 0;
        else if (currentGunIndex < 0)
            currentGunIndex = 2;
    }
    private void UpdateGunSprite()
    {
        spriteRenderer.sprite = guns[currentGunIndex].gunSprite;
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
