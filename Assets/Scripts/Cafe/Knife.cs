using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CutFruit>() != null)
        {
            CutFruit cutFruit = other.GetComponent<CutFruit>();
            cutFruit.Cut();
        }
    }
}
