using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Gun
{
    int _ammoMax;
    public int _ammoCurrent;
    int _magazineMax;
    public int _magazineCurrent;
    int _bulletsPerShot;
    float _range;
    float _timeBetweenShots;
    float _timeBetweenShotsCounter;
    float _spread;
    public float _reloadDuration;
    public float _reloadTimer;
    float _damagePerBullet;
    bool _canHoldTrigger;
    public bool _isReloading;
    public Sprite gunSprite;

    public Gun(bool canHoldTrigger, float timeBetweenShots, float damagePerBullet, float spread, int bulletsPerShot, float range, int ammoMax, int magazineMax, float reloadDuration, Sprite Sprite)
    {
        _canHoldTrigger = canHoldTrigger;
        _timeBetweenShots = timeBetweenShots;
        _damagePerBullet = damagePerBullet;
        _spread = spread;
        _bulletsPerShot = bulletsPerShot;
        _range = range;
        _ammoMax = ammoMax;
        _magazineMax = magazineMax;
        _reloadDuration = reloadDuration;
        gunSprite = Sprite;
    }
    
    public void Shoot(Vector3 originPosition, LayerMask mask)
    {
        _timeBetweenShotsCounter -= Time.deltaTime;
        if (_canHoldTrigger)
        {
            if (Input.GetKey(KeyCode.Mouse0) && _timeBetweenShotsCounter <= 0 && _magazineCurrent > 0)
            {
                _Shoot(originPosition, mask);
            }
        }
        else if (!_canHoldTrigger)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && _timeBetweenShotsCounter <= 0 && _magazineCurrent > 0)
            {
                _Shoot(originPosition, mask);
            }
        }
    }
    public void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && _ammoCurrent > 0 && _magazineCurrent < _magazineMax)
        {
            int reloadAmount = _magazineMax - _magazineCurrent;
            if (_ammoCurrent < reloadAmount) reloadAmount = _ammoCurrent;
            _ammoCurrent -= reloadAmount;
            _magazineCurrent += reloadAmount;
            _reloadTimer = _reloadDuration;
            _isReloading = true;
        }
    }
    public void ReloadOnAwake()
    {
        _ammoCurrent = _ammoMax;
        _magazineCurrent = _magazineMax;
    }
    private void _Shoot(Vector3 originPosition, LayerMask mask)
    {
        _timeBetweenShotsCounter = _timeBetweenShots;
        _magazineCurrent--;
        for (int i = 0; i < _bulletsPerShot; i++)
        {
            Vector2 aimDirectionSpread = new Vector2(WeaponManager.aimDirection.x + Random.Range(-_spread, _spread), WeaponManager.aimDirection.y + Random.Range(-_spread, _spread));
            aimDirectionSpread.Normalize();
            Debug.DrawRay(originPosition, aimDirectionSpread * _range, Color.cyan, 0.1f);

            RaycastHit2D hit;

            if(Physics2D.Raycast(originPosition, aimDirectionSpread, _range, mask)) // Checks if Raycast hit in order to prevent error.
            {
                hit = Physics2D.Raycast(originPosition, aimDirectionSpread, _range, mask);
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.collider.gameObject.GetComponent<TestEnemy>().health -= _damagePerBullet;
                }
            }
        }
    }
}