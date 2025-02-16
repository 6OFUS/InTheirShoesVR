/*
* Author: Malcom Goh
* Date: 15/2/2025
* Description: Customer AI and navigation
* */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    /// <summary>
    /// The NavMeshAgent used for customer navigation.
    /// </summary>
    public NavMeshAgent customer;

    /// <summary>
    /// The destination where the customer will walk to, such as the front of the counter.
    /// </summary>
    public Transform frontOfCounter;

    /// <summary>
    /// The destination where the customer will return after completing the order.
    /// </summary>
    public Transform returnToTable;

    /// <summary>
    /// The tray held by the customer while at the counter.
    /// </summary>
    public GameObject trayHand;

    /// <summary>
    /// The fruit on the tray the customer holds.
    /// </summary>
    public GameObject trayHandFruit;

    /// <summary>
    /// The tray placed on the counter.
    /// </summary>
    public GameObject trayCounter;

    /// <summary>
    /// The animator for controlling the customer's animations.
    /// </summary>
    public Animator customerAnimator;

    /// <summary>
    /// Makes the customer walk to the front of the counter.
    /// </summary>
    public void WalkToCounter()
    {
        customer.SetDestination(frontOfCounter.position);
        StartCoroutine(CheckIfArrived(customerAnimator.GetCurrentAnimatorStateInfo(0).length));
    }

    /// <summary>
    /// Coroutine that checks if the customer has arrived at the counter and triggers animations.
    /// </summary>
    /// <param name="delay">The delay after the customer arrives before performing the next action.</param>
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

    /// <summary>
    /// Coroutine that handles the customer's idle animation cycle.
    /// </summary>
    public IEnumerator PointIdleCycle()
    {
        customerAnimator.SetTrigger("Point");
        yield return new WaitForSeconds(customerAnimator.GetCurrentAnimatorStateInfo(0).length);
        customerAnimator.SetTrigger("Idle");
        yield return new WaitForSeconds(customerAnimator.GetCurrentAnimatorStateInfo(0).length);
    }

    /// <summary>
    /// Coroutine that returns the customer to their table after completing the order.
    /// </summary>
    public IEnumerator ReturnToTable()
    {
        customerAnimator.SetTrigger("Order complete");
        trayHand.SetActive(true);
        trayHandFruit.SetActive(true);
        trayCounter.SetActive(false);
        customer.SetDestination(returnToTable.position);
        customerAnimator.SetTrigger("Walk");
        yield return null;
    }

    /// <summary>
    /// Called at the start of the game, triggers the customer to walk to the counter.
    /// </summary>
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
