using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour,IPointerEnterHandler
{
    
    [SerializeField]private AudioSource ButtonHighlighted;
    [SerializeField]private AudioSource ButtonSelected;
    
    [SerializeField]private AudioClip HighlightedSound;
    [SerializeField]private AudioClip SelectedSound;
    
    //サウンドの設定
    public void PlayHighlightedSound()
    {
        ButtonHighlighted.PlayOneShot(HighlightedSound);
    }

    public void PlaySelectedSound()
    {
        Debug.Log("ぬん");
        ButtonSelected.PlayOneShot(SelectedSound);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("起動");
        PlayHighlightedSound();
    }
}
