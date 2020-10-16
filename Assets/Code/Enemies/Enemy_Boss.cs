using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : MonoBehaviour
{
    private GameObject player;


    [SerializeField] private float bossHp;


    [SerializeField] private float meleeCooldown;
    private float _meleeCooldown;
    [SerializeField] private float meleeRange;

    [SerializeField] private float rangedCooldown;
    private float _rangedCooldown;

    [SerializeField] private float specialCooldown;
    private float _specialCooldown;

    [SerializeField] private float attackCooldown;
    private float _attackCooldown;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }
    void Update()
    {
        CooldownsCounter();
        Attack();
    }
    private void CooldownsCounter()
    {
        _attackCooldown -= Time.deltaTime;
        _meleeCooldown -= Time.deltaTime;
        _rangedCooldown -= Time.deltaTime;
        _specialCooldown -= Time.deltaTime;
    }
    private void Attack()
    {
        if (_attackCooldown <= 0)
        {
            if (_specialCooldown <= 0)
            {
                Attack_Special();
            }
            else if (_meleeCooldown <= 0 && Vector2.Distance(this.transform.position, player.transform.position) < meleeRange)
            {
                Attack_Melee();
            }
            else if (_rangedCooldown <= 0)
            {
                Attack_Ranged();
            }
        }
    }
    private void Attack_Melee()
    {
        print("melee");
        _meleeCooldown = meleeCooldown;
        _attackCooldown = attackCooldown;
    }
    private void Attack_Ranged()
    {
        print("ranged");
        _rangedCooldown = rangedCooldown;
        _attackCooldown = attackCooldown;
    }
    private void Attack_Special()
    {
        print("special");
        _specialCooldown = specialCooldown;
        _attackCooldown = attackCooldown;
    }

    public void Damage(float amount)
    {
        bossHp -= amount;
        if (bossHp <= 0)
        {
            Destroy(this.gameObject);
            // Animação de morte?
            // Dá pontos ao jogador?
            // GG?
        }
    }
}
