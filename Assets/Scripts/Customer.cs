using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public NavMeshAgent customer;

    public Transform frontOfCounter;

    public GameObject order;

    public void WalkToCounter()
    {
        customer.SetDestination(frontOfCounter.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        WalkToCounter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
