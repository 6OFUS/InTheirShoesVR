using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public NavMeshAgent customer;

    public Transform frontOfCounter;

    public GameObject trayHand;

    public Animator customerAnimator;

    public void WalkToCounter()
    {
        customer.SetDestination(frontOfCounter.position);
        StartCoroutine(CheckIfArrived());
    }

    private IEnumerator CheckIfArrived()
    {
        float threshold = 1f;
        yield return new WaitForSeconds(0.1f); // Small delay to let path calculate

        while (customer.pathPending || customer.remainingDistance > threshold)
        {
            yield return null; // Wait until NPC reaches the destination

        }
        customerAnimator.SetTrigger("At counter");
        yield return new WaitForSeconds(customerAnimator.);
        trayHand.SetActive(false);
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
