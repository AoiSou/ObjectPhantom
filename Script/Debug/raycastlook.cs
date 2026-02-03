using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastlook : MonoBehaviour
{
    [SerializeField] private float RaycastDistance = 100f;

    void Update()
    {
        Vector3 origin = transform.position; // オブジェクトの現在位置
        Vector3 direction = transform.forward;
        
        Debug.DrawRay(origin, direction * RaycastDistance, Color.red);
    }
}
