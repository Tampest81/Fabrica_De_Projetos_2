using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    private ContactFilter2D filter;
    private Collider2D[] hit = new Collider2D[1];

    private void Start()
    {
        filter.layerMask = mask;
    }
    void Update()
    {
        Physics2D.OverlapCollider(this.GetComponent<Collider2D>(), filter, hit);
        if (hit[0].tag == "Player")
        {
            hit[0].GetComponent<PlayerMovement>().TakeDamage(1);
            Destroy(this.gameObject);
        }
    }
}
