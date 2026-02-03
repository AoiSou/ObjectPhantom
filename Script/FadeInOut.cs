using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image White;
    public float Speed;
    
    public static bool OnFadeOut;
    public static bool OnFadeIn;

    void Start()
    {
        White=GetComponent<UnityEngine.UI.Image>();
    }

    void FixedUpdate()
    {
        if (OnFadeOut)
        {
            FadeOut();
        }
        if (OnFadeIn)
        {
            FadeIn();
        }
    }

    void FadeIn()
    {
        Color c = White.color;
        c.a = White.color.a + Speed * Time.deltaTime;
        White.color = c;
        if (White.color.a >= 1.0f)
        {
            Debug.Log(White.color.a);
            OnFadeIn = false;
            OnFadeOut = true;
        }
    }
    
    void FadeOut()
    {
        Color c = White.color;
        c.a = White.color.a - Speed * Time.deltaTime;
        White.color = c;
        if (White.color.a <= 0)
        {
            Time.timeScale = 1;
            c.a = 0;
            White.color = c;
            OnFadeOut = false;
        }
    }
}
