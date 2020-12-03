using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Attacks : MonoBehaviour
{
    private Animator animator;

    public Animator winAnim;
    private GameObject player;


    private float bossHpVariation;
    public float bossHp;

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
    [SerializeField] private GameObject meleeShockwave;
    [SerializeField] private float meleeShockwaveSpeed;
    [SerializeField] private Transform shockWaveOriginPos;

    private bool attacking = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = this.GetComponent<Animator>();

        filter.layerMask = mask;
    }
    void Update()
    {
        CooldownsCounter();
        Attack();
        DamageFlash();
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
        if (player)
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
    }
    private void Attack_Melee()
    {
        _meleeCooldown = meleeCooldown;
        _attackCooldown = attackCooldown;

        var tmp1 = Instantiate(meleeShockwave, shockWaveOriginPos.position, Quaternion.identity);
        tmp1.GetComponent<Rigidbody2D>().velocity = new Vector3(-1, 0) * meleeShockwaveSpeed;

        var tmp2 = Instantiate(meleeShockwave, shockWaveOriginPos.position, Quaternion.identity);
        tmp2.GetComponent<Rigidbody2D>().velocity = new Vector3(1, 0) * meleeShockwaveSpeed;
        tmp2.transform.rotation = Quaternion.Euler(0, 180, 0);

        Destroy(tmp1, 2);
        Destroy(tmp2, 2);


        Physics2D.OverlapCollider(meleeHitbox, filter, meleeHit);

        if (meleeHit[0] && meleeHit[0].CompareTag("Player"))
        {
            MonoBehaviour.print("MeleeHit");
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

    // Yey

    private void OnDestroy()
    {
        winAnim.Play("Vitory"); // é ta escrito errado , pq na animação eu coloquei o nome errado...
    }

    private void CameraShake()
    {
        StartCoroutine(_cameraShake(5, 1));
    }

    [SerializeField] private CinemachineVirtualCamera cam;
    private IEnumerator _cameraShake(float strength, float duration)
    {
        var camNoise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        float decrement = strength / duration;

        for (float i = duration; i > 0; i -= Time.deltaTime)
        {
            strength -= decrement * Time.deltaTime;
            camNoise.m_AmplitudeGain = strength;
            yield return null;
        }

        camNoise.m_AmplitudeGain = 0;
    }


    [SerializeField] private GameObject damageFlashObj;
    private Animator damageFlashAnimator;
    private void DamageFlash()
    {
        damageFlashAnimator = damageFlashObj.GetComponent<Animator>();
        if (bossHpVariation != bossHp)
        {
            damageFlashAnimator.Play("Boss_Damage_Flash");
        }
        bossHpVariation = bossHp;
    }
}