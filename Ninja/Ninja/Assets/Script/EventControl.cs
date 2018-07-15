using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventControl : MonoBehaviour
{
    [SerializeField]
    private float eventpos = 120.0f;
    [SerializeField]
    private GameObject PlayerConrol;

    private GameObject bossControl;
    private PlayerControl playerHP;
    private BossControl bossHP;

    private string m_nowScene;

    private void Start()
    {
        playerHP = GetComponent<PlayerControl>();
        bossControl = GameObject.Find("BossControl");
        bossHP = GetComponent<BossControl>();
    }

    void Update ()
    {
        m_nowScene = SceneManager.GetActiveScene().name;

        switch (m_nowScene)
        {
            case "Title":
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SceneManager.LoadScene("main");
                }
                break;
            case "main":
                MainScreenTransition();
                break;
            case "Boss":
                BossScreenTransition();
                break;
            case "GameClear":
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SceneManager.LoadScene("Title");
                }
                break;
            case "GameOver":
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    SceneManager.LoadScene("Title");
                }
                break;
        }
	}

    private void MainScreenTransition()
    {
        if (PlayerConrol.transform.position.x > eventpos)
            SceneManager.LoadScene("Boss");
        if (0 == playerHP.life)
            SceneManager.LoadScene("GameOver");
    }

    private void BossScreenTransition()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
           // bossHP.life = 0;
            SceneManager.LoadScene("GameClear");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
           // playerHP.life = 0;
            SceneManager.LoadScene("GameOver");
        }
        //if (0 == bossHP.life)
        //    SceneManager.LoadScene("GameClear");
        if (0 == playerHP.life)
            SceneManager.LoadScene("GameOver");
    }
}
