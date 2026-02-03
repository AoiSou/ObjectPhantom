using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
     public static int successcount;
     [SerializeField] int successMaxPoint;
     [SerializeField] private TextMeshProUGUI successcounttext;
    [SerializeField] private GameObject gameovercanvas;
    [SerializeField] private GameObject gameover;
    
    [SerializeField] Canvas canvas;
    
    public bool isTutorialMode = false;

    void Start()
    {
        canvas.gameObject.SetActive(false);
        successcount = 0;
        Time.timeScale = 1;
    }
    private void Update()
    {
        if(!ModeChange.OnTutorialStop)
            PauseMenu();
        if(!isTutorialMode)
            successcounttext.text = ("あと"+(successMaxPoint -successcount)+"体の立方体の敵を倒せ");
        else
            successcounttext.text = "";
        
        successedcount();
        
    }
    void successedcount()
    {
        if (successcount == successMaxPoint)
        {
            gameovercanvas.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            gameovercanvas.SetActive(false);
        }
    }

    void PauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas.gameObject.activeSelf)
            {
                canvas.gameObject.SetActive(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 1;
            }
            else
            {
                canvas.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 0;
            }
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    void GameOver()
    {
        gameover.SetActive(true);
    }
}
