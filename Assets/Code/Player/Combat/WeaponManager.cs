using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;

public class WeaponManager : MonoBehaviour
{
    public static Vector2 aimDirection;

    [SerializeField] Sprite pistolSprite;
    [SerializeField] Sprite shotgunSprite;
    [SerializeField] Sprite machinegunSprite;

    public Gun[] guns = new Gun[3];
    private Gun pistol = new Gun(false, 0.3f, 1, 0, 1, 10, 20, 5, 0.5f, null);
    private Gun shotgun = new Gun(false, 0.3f, 1, 0.3f, 10, 7.5f, 10, 2, 1, null);
    private Gun machinegun = new Gun(true, 0.05f, 1, 0.1f, 1, 10, 200, 50, 2, null);
    public int currentGunIndex;

    SpriteRenderer spriteRenderer;

    RaycastHit2D[] raycastResults = new RaycastHit2D[1];

    [SerializeField] private LayerMask layersToHit;

    private void Awake()
    {
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
                guns[currentGunIndex].Shoot(this.transform.position, layersToHit);
                guns[currentGunIndex].Reload();

                break;
        }
        Aim();
        UpdateGunSprite();

        Debug.DrawRay(this.transform.position, aimDirection*10, Color.red);
    }
    private void Aim()
    {
        aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        aimDirection.Normalize();
        float rotZ = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
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
        print (this.transform.rotation.eulerAngles);
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
