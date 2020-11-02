using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // esse script é só para complementar a hud por em quanto, já que não existe um dedicado ainda para o jogador.
    public float hp = 0;
    public float score = 0;
    private PlayerMovement player;
    public HUD_Manager hud;

    private void Start()
    {
        player = GetComponent<PlayerMovement>();
        hp = player.health;
    }
    private void Update()
    {
        hp = player.health;

        if(hp <= 0)
        {
            hud.GameOver();
        }
    }

    private void OnDestroy()
    {
        hud.GameOver();
    }
}
