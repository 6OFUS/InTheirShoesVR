using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public NavMeshAgent customer;

    public Transform frontOfCounter;

    public GameObject trayHand;
    public GameObject trayCounter;

    public Animator customerAnimator;
    public bool receiptPickedUp;

    public void WalkToCounter()
    {
        customer.SetDestination(frontOfCounter.position);
        StartCoroutine(CheckIfArrived(customerAnimator.GetCurrentAnimatorStateInfo(0).length));
    }

    private IEnumerator CheckIfArrived(float delay)
    {
        float threshold = 1f;
        yield return new WaitForSeconds(0.1f); // Small delay to let path calculate

        while (customer.pathPending || customer.remainingDistance > threshold)
        {
            yield return null; // Wait until NPC reaches the destination

        }
        customerAnimator.SetTrigger("At counter");
        yield return new WaitForSeconds(delay);
        trayHand.SetActive(false);
        trayCounter.SetActive(true);
        StartCoroutine(PointIdleCycle());
    }

    private IEnumerator PointIdleCycle()
    {
        while (!receiptPickedUp)
        {
            customerAnimator.SetTrigger("Point");
            yield return new WaitForSeconds(customerAnimator.GetCurrentAnimatorStateInfo(0).length);
            customerAnimator.SetTrigger("Idle");
            yield return new WaitForSeconds(customerAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void PickupReceipt()
    {
        receiptPickedUp = true;
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
