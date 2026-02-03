using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   

public class ChaseAgent : MonoBehaviour
{
    [SerializeField] private GameObject target;
    
    [SerializeField] private NavMeshAgent agent;
    
    void Update()
    {
        if (agent.enabled)
        {
            agent.destination =target.transform.position;
        }
    }
    
    void Start()
    {
        agent.enabled = false;
    }
    
    
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            agent.enabled=true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            agent.enabled = false;
        }
    }
}
