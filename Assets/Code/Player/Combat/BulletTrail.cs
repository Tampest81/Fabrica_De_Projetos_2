using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private Renderer spriteRenderer; 
    void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, 0.25f);
    }
    void Update()
    {
        spriteRenderer.material.color = Color.Lerp(spriteRenderer.material.color, Color.clear, Time.deltaTime * 25);
    }
}
