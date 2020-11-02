using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Zombie : Enemy
{
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
        _checkBoxPosition = this.transform.position + checkBoxPosition;
    }
    void Update()
    {
        _checkBoxPosition = new Vector3(this.transform.position.x + checkBoxPosition.x, this.transform.position.y);
        if (waiting)
        {
            Wait();
        }
        else
        {
            WaitCheck();
        }
    }
    private void FixedUpdate()
    {
        if (!waiting)
        {
            Move();
        }
    }
    private void OnDrawGizmos() // Visualização dos raycasts;
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(_checkBoxPosition, checkBoxSize);
    }
}