using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialRespawn : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject PlayerCam;
    
    [SerializeField] GameObject RespawnPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            FadeInOut.OnFadeIn = true;
            Invoke("Respawn", 0.8f);
        }
    }

    void Respawn()
    {
        PlayerObject.transform.position = RespawnPoint.transform.position;
        PlayerCam.transform.position = PlayerObject.transform.position;
    }
}
