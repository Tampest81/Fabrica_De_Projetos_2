﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Gun
{
    int _ammoMax;
    public int _ammoCurrent;
    public int _magazineMax;
    public int _magazineCurrent;
    int _bulletsPerShot;
    public float _range;
    float _timeBetweenShots;
    float _timeBetweenShotsCounter;
    float _spread;
    public float _reloadDuration;
    public float _reloadTimer;
    float _damagePerBullet;
    float _damageFallOff;
    public bool _canHoldTrigger;
    public bool _isReloading;
    private bool _isRaycast;
    public GameObject _projectilePrefab;
    private float _projectileSpeed;
    public Sprite gunSprite;

    public Gun(bool isRaycast, bool canHoldTrigger, float timeBetweenShots, float damagePerBullet, float spread, int bulletsPerShot, float range, int ammoMax, int magazineMax, float reloadDuration, GameObject projectilePrefab, float projectileSpeed, Sprite sprite)
    {
        _isRaycast = isRaycast;
        _canHoldTrigger = canHoldTrigger;
        _timeBetweenShots = timeBetweenShots;
        _damagePerBullet = damagePerBullet;
        _spread = spread;
        _bulletsPerShot = bulletsPerShot;
        _range = range;
        _ammoMax = ammoMax;
        _magazineMax = magazineMax;
        _reloadDuration = reloadDuration;
        _projectilePrefab = projectilePrefab;
        _projectileSpeed = projectileSpeed;
        gunSprite = sprite;
    }

    public void Shoot(Vector3 originPosition, Vector3 aimDirection, LayerMask mask, bool isPlayer)
    {
        _timeBetweenShotsCounter -= Time.deltaTime;
        if (isPlayer)
        {
            if (_canHoldTrigger)
            {
                if (Input.GetKey(KeyCode.Mouse0) && _timeBetweenShotsCounter <= 0)
                {
                    if (_magazineCurrent > 0)
                    {
                        _Shoot(originPosition, aimDirection, mask);
                    }
                    else 
                    {
                    }
                }
            }
            else if (!_canHoldTrigger)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && _timeBetweenShotsCounter <= 0 && _magazineCurrent > 0)
                {
                    _Shoot(originPosition, aimDirection, mask);
                }
            }
        }
        else 
        {
            _Shoot(originPosition, aimDirection, mask);
        }
    }

    public void Reload()
    {
        
        int reloadAmount = _magazineMax - _magazineCurrent;
        if (_ammoCurrent < reloadAmount) reloadAmount = _ammoCurrent;
        _ammoCurrent -= reloadAmount;
        _magazineCurrent += reloadAmount;
        _reloadTimer = _reloadDuration;
        _isReloading = true;
    }
    public void ReloadOnAwake()
    {
        _ammoCurrent = _ammoMax;
        _magazineCurrent = _magazineMax;
    }
    public void CollectAmmo(int amount)
    {
        _ammoCurrent += amount;
        if (_ammoCurrent > _ammoMax)
        {
            _ammoCurrent = _ammoMax;
        }
    }

    private WeaponManager _weaponManager;

    private void _Shoot(Vector3 originPosition, Vector3 aimDirection, LayerMask mask)
    {
        _timeBetweenShotsCounter = _timeBetweenShots;
        _magazineCurrent--;
        for (int i = 0; i < _bulletsPerShot; i++)
        {
            
            Vector2 aimDirectionSpread = new Vector2(aimDirection.x + Random.Range(-_spread, _spread), aimDirection.y + Random.Range(-_spread, _spread));
            aimDirectionSpread.Normalize();

            if (_isRaycast)
            {
                ShootVisuals(originPosition, aimDirectionSpread);

                Debug.DrawRay(originPosition, aimDirectionSpread * _range, Color.cyan, 0.1f);

                RaycastHit2D hit;

                if (Physics2D.Raycast(originPosition, aimDirectionSpread, Mathf.Infinity, mask)) // Checks if Raycast hit in order to prevent error.
                {
                    hit = Physics2D.Raycast(originPosition, aimDirectionSpread, Mathf.Infinity, mask);
                    HitVisuals(hit.point, hit);

                    _damageFallOff = _damagePerBullet;
                    if (hit.distance > _range)
                    {
                        _damageFallOff /= 2;
                    }

                    if (hit.collider.CompareTag("Enemy") && hit.distance <= (_range * 2))
                    {
                        if (hit.collider.GetComponent<Enemy_Sentry>())
                            hit.collider.gameObject.GetComponent<Enemy_Sentry>().TakeDamage(_damageFallOff);

                        else if (hit.collider.GetComponent<Enemy_Zombie>())
                            hit.collider.gameObject.GetComponent<Enemy_Zombie>().TakeDamage(_damageFallOff);

                        else if (hit.collider.GetComponent<Enemy_Melee>())
                            hit.collider.gameObject.GetComponent<Enemy_Melee>().TakeDamage(_damageFallOff);

                        else if (hit.collider.GetComponent<Enemy_Flying>())
                            hit.collider.gameObject.GetComponent<Enemy_Flying>().TakeDamage(_damageFallOff);

                        else if (hit.collider.GetComponent<Boss_Attacks>())
                            hit.collider.gameObject.GetComponent<Boss_Attacks>().Damage(_damageFallOff);

                        else if (hit.collider.GetComponent<Boss_Minion_Homing>())
                            hit.collider.GetComponent<Boss_Minion_Homing>().TakeDamage();

                        else if (hit.collider) ;
                            //hit.collider.gameObject.GetComponent<TestEnemy>().health -= _damageFallOff;
                    }
                }
            }
            else
            {
                var projectile = MonoBehaviour.Instantiate(_projectilePrefab, originPosition, Quaternion.Euler(aimDirectionSpread));
                projectile.GetComponent<Rigidbody2D>().velocity = aimDirectionSpread * _projectileSpeed;
                MonoBehaviour.Destroy(projectile, 2);
            }
        }
    }

    private void ShootVisuals(Vector3 originPos, Vector3 dir)
    {
        int i = Random.Range(0, WeaponManager.muzzleFlash.Length);
        var tmp = GameObject.Instantiate(WeaponManager.muzzleFlash[i], originPos, Quaternion.Euler(0, 0, WeaponManager.rotZ - 90));
        var tmp2 = GameObject.Instantiate(WeaponManager.bulletTrail, originPos, Quaternion.identity);
        tmp2.GetComponent<Rigidbody2D>().AddForce(dir * 10000);
        GameObject.Destroy(tmp, 0.1f);
        GameObject.Destroy(tmp2, 0.5f);
    }
    private void HitVisuals(Vector3 originPos, RaycastHit2D hit)
    {
        int i = Random.Range(0, WeaponManager.muzzleFlash.Length);
        GameObject tmp = null;

        float rotZ = Mathf.Atan2(hit.normal.y, hit.normal.x) * Mathf.Rad2Deg;

        tmp = GameObject.Instantiate(WeaponManager.muzzleFlash[i], originPos, Quaternion.Euler(0, 0, rotZ - 90));

        GameObject.Destroy(tmp, 0.1f);
    }
}