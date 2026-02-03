using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TutorialText;
    [SerializeField] string TutoText;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            TutorialText.text = TutoText;
        }
    }
}
