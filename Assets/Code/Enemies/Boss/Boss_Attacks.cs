using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attacks : MonoBehaviour
{
    private Animator animator;


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

    [SerializeField] private Collider2D meleeHitbox;
    Collider2D[] meleeHit = new Collider2D[1];
    [SerializeField] LayerMask mask;
    ContactFilter2D filter;

    private bool attacking = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player"); // Adaptação pq eh um projeto diferente
        animator = this.GetComponent<Animator>();

        filter.layerMask = mask;
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
            if (_specialCooldown <= 0 && !attacking)
            {
                animator.Play("Boss_Attack_Special");
                attacking = true;
            }
            else if (_meleeCooldown <= 0 && Vector2.Distance(this.transform.position, player.transform.position) < meleeRange && !attacking)
            {
                animator.Play("Boss_Attack_Melee");
                attacking = true;
            }
            else if (_rangedCooldown <= 0 && !attacking)
            {
                animator.Play("Boss_Attack_Ranged");
                attacking = true;
            }
        }
    }
    private void Attack_Melee()
    {
        _meleeCooldown = meleeCooldown;
        _attackCooldown = attackCooldown;

        Physics2D.OverlapCollider(meleeHitbox, filter, meleeHit);

        if (meleeHit[0] && meleeHit[0].CompareTag("Player"))
        {
            meleeHit[0].GetComponent<PlayerMovement>().TakeDamage(3, true);
        }
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

    // Ataque Ranged e Configs do mesmo //
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileOriginPos;
    private Vector3 projectileDirection;

    private void Attack_Ranged()
    {
        var tmpProjectile = Instantiate(projectile, projectileOriginPos.position, projectileOriginPos.transform.rotation);
        Destroy(tmpProjectile, 5);

        print("ranged");
        _rangedCooldown = rangedCooldown;
        _attackCooldown = attackCooldown;
    }

    // Ataque Especial e Configs do Mesmo //
    [SerializeField] private int numberOfSpawns;
    [SerializeField] private float timeBetweenSpawns;
    private float spawnsCount;
    [SerializeField] private GameObject toSpawn;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float spawnDirSpread;
    

    private void Attack_Special()
    {
        Invoke("SpecialSpawn", 0);

        print("special");
        _specialCooldown = specialCooldown;
        _attackCooldown = attackCooldown;
    }

    private void SpecialSpawn()
    {
        if(spawnsCount < numberOfSpawns)
        {   
            var tmp = Instantiate(toSpawn, spawnPos.position, Quaternion.identity);
            Destroy(tmp, 10);
            Invoke("SpecialSpawn", timeBetweenSpawns);

            Vector2 enemyMoveDir = spawnPos.up + new Vector3(0 + Random.Range(-spawnDirSpread, spawnDirSpread), 0);
            enemyMoveDir.Normalize();

            tmp.GetComponent<Rigidbody2D>().AddForce(enemyMoveDir * 100);
        }
        else
        {
            spawnsCount = 0;
        }
        spawnsCount++;
    }

    private void EndAttack()
    {
        attacking = false;
    }
}