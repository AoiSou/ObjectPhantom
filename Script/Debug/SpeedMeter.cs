using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    public Rigidbody rb;
    
    [SerializeField] TextMeshProUGUI text;

    void Update()
    {
        // Rigidbody の速度ベクトルからスピードを計算
        float speed = rb.velocity.magnitude;
        
        text.text = Mathf.RoundToInt(speed).ToString();
    }
}
