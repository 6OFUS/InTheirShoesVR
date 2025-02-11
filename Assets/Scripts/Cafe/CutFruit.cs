using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutFruit : MonoBehaviour
{
    public GameObject wholeFruit;
    public GameObject[] cutFruits;

    public void Cut()
    {
        wholeFruit.SetActive(false);
        foreach(GameObject slice in cutFruits)
        {
            slice.SetActive(true);
        }
    }
}
