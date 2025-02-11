using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSocket : MonoBehaviour
{
    public string orderNum;

    public GameObject correctFruit;

    public bool correct;

    private void OnTriggerEnter(Collider other)
    {
        if (correctFruit == other.gameObject)
        {
            correct = true;
        }
        else
        {
            correct = false;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
