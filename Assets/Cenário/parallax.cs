﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public Transform cam;
    public Transform[] camadas;
    public float[] mult;
    private Vector3[] posOriginal;

    
    // Start is called before the first frame update
    void Start()
    {
        posOriginal = new Vector3[camadas.Length];
        for( int i = 0; i < camadas.Length; i++)
        {
            posOriginal[1] = camadas[i].position;
        }
    }

    // Update is called once per frame
    void Update()
    {
       for ( int i = 0; i< camadas.Length; i ++)
        {
            camadas[i].position = posOriginal[i] + mult[i] * (new Vector3(cam.position.x, cam.position.y, camadas[i].position.z));
        }
    }
}
