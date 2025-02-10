using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSocket : MonoBehaviour
{
    private char storedLetter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Letter Block"))
        {
            LetterBlock block = other.GetComponent<LetterBlock>();
            storedLetter = block.letter;
            Debug.Log(storedLetter);
        }
    }

    public void SetLetter(char letter)
    {
        storedLetter = letter;
    }

    public char GetLetter()
    {
        return storedLetter;
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
[Serializable]
public class LetterSocketGroup
{
    public LetterSocket[] letterSockets;
}
