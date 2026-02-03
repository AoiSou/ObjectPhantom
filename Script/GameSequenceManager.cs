using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSequenceManager : MonoBehaviour
{
    [SerializeField] private GameObject Quits;
    [SerializeField] private GameObject Credits;
    [SerializeField] private GameObject Options;

    void Start()
    {
        Time.timeScale = 1;
    }
    
    public void StartGame()
    {
        Invoke("LoadStart", 0.5f);
    }

    public void title()
    {
        SceneManager.LoadScene("00Title");
    }

    public void Option()
    {
        if (!Options.activeSelf)
             Options.SetActive(true);
        else
            Options.SetActive(false);
    }

    public void Credit()
    {
        if (!Credits.activeSelf)
            Credits.SetActive(true);
        else
            Credits.SetActive(false);
    }
    
    void LoadStart()
    {
        SceneManager.LoadScene("01Stage");
    }

    public void QuitButton()
    {
        if (!Quits.activeSelf)
            Quits.SetActive(true);
        else
            Quits.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame");
        Application.Quit();
        //しね↓二度と使うな
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
