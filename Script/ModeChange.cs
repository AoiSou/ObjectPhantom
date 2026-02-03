using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class ModeChange : MonoBehaviour
{
   [SerializeField] GameObject TutorialFinish;
   [SerializeField] TextMeshProUGUI successText;
   
   [SerializeField] GameObject PlayerObject;
   [SerializeField] GameObject PlayerCam;

   [SerializeField] GameObject TeleportPoint;
   
   public static bool OnTutorialStop=false;
   
   void Start()
   {
      TutorialFinish.gameObject.SetActive(false);
      successText.gameObject.SetActive(false);
   }
   
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         OnTutorialStop = true;
         Cursor.visible = true;
         Cursor.lockState = CursorLockMode.None;
         TutorialFinish.gameObject.SetActive(true);
         Time.timeScale = 0;
      }
   }
   
   public void NoButton()
   {
      //Debug.Log("Clicked NoButton");
      OnTutorialStop=false;
      TutorialFinish.gameObject.SetActive(false);
      Cursor.lockState = CursorLockMode.Locked;
      Time.timeScale = 1;
   }

   public void PlayerTeleport()
   {
      Cursor.lockState = CursorLockMode.Locked;
      
      PlayerObject.transform.position = TeleportPoint.transform.position;
      PlayerCam.transform.position = TeleportPoint.transform.position;
      successText.gameObject.SetActive(true);
      
   }
   
   public void YesButton()
   {
      FadeInOut.OnFadeIn = true;
      Time.timeScale = 1;
      TutorialFinish.gameObject.SetActive(false);
      Invoke("PlayerTeleport",0.8f);
      OnTutorialStop=false;
   }
}
