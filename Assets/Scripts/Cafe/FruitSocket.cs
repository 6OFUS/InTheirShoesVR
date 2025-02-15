using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FruitSocket : MonoBehaviour
{
    public string orderNum;

    public GameObject correctFruit;

    public bool correct;


    public void CheckFruit(SelectEnterEventArgs args)
    {
        GameObject placedFruit = args.interactableObject.transform.gameObject;

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
