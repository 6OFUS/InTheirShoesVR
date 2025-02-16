/*
* Author: Jennifer Lau
* Date: 15/2/2025
* Description: Fruit socket script for checking placed fruit against the correct fruit order.
* */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FruitSocket : MonoBehaviour
{
    /// <summary>
    /// The order number associated with this fruit socket.
    /// </summary>
    public string orderNum;

    /// <summary>
    /// The correct fruit object that should be placed in this socket.
    /// </summary>
    public GameObject correctFruit;

    /// <summary>
    /// The fruit that has been placed in the socket by the player.
    /// </summary>
    public GameObject placedFruit;

    /// <summary>
    /// A boolean flag indicating whether the correct fruit is placed in the socket.
    /// </summary>
    public bool correct;

    /// <summary>
    /// The customer associated with this fruit socket.
    /// </summary>
    public Customer customer;

    /// <summary>
    /// Checks if the fruit placed in the socket is correct by comparing it to the correct fruit.
    /// This method is called when a fruit is placed in the socket.
    /// </summary>
    /// <param name="args">The event arguments containing the interactable object (fruit) being placed in the socket.</param>
    public void CheckFruit(SelectEnterEventArgs args)
    {
        placedFruit = args.interactableObject.transform.gameObject;

        if (placedFruit.CompareTag(correctFruit.tag))
        {
            correct = true;
            Debug.Log("Correct fruit placed");
        }
        else
        {
            correct = false;
            Debug.Log("Incorrect fruit placed");
        }
    }


}
