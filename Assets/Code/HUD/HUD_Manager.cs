using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD_Manager : MonoBehaviour
{
    public static HUD_Manager HudManager;

    private void Awake()
    {
        if (!HudManager)
        {
            HudManager = this;
            DontDestroyOnLoad(this);
        }

    }
    [Header("Player Script")]
    public Player player;

    [Header("Sliders")]
    public Slider life;

    [Header("Pause And GameOver GameObjects")]
    public GameObject PauseScreen;
    public GameObject GameOverGameObject;

    bool Switch = false;

    private void Start()
    {
        life.maxValue = player.hp;
    }

    private void Update()
    {
        //HUD Txt Updater \/\/\/\/
        life.value = player.hp;
        //Hud Txt Updater /\/\/\/\

        //Pause And Unpause System \/\/\/\/\/\/\/\/\/\/
        if (Input.GetKeyDown(KeyCode.Escape) & Switch == false)
        {
            PauseScreen.SetActive(true);
            Switch = true;
            //Game Pause
            GamePause();
        }

        else if (Input.GetKeyDown(KeyCode.Escape) & Switch == true)
        {
            PauseScreen.SetActive(false);
            Switch = false;
            //Game Resume
            GameResume();
        }
        //Pause And Unpause System /\/\/\/\/\/\/\/\/\


        if (player.hp <= 0)
        {
            GamePause();
            GameOverGameObject.SetActive(true);
        }
    }





    // Hud Methods;

    public void Restart()
    {
        player.hp = 3;
        GameResume();
        GameOverGameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GamePause()
    {
        Time.timeScale = 0;
    }

    public void GameResumeButton()
    {
        Time.timeScale = 1;
        PauseScreen.SetActive(false);
    }

    public void GameResume()
    {
        Time.timeScale = 1;
    }

}
