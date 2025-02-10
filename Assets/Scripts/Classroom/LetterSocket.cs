using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSocket : MonoBehaviour
{
    public char storedLetter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Letter Block"))
        {
            LetterBlock block = other.GetComponent<LetterBlock>();
            storedLetter = block.letter;
            Debug.Log(storedLetter);
        }
    }
    public char GetLetter()
    {
        return storedLetter;
    }
}

[Serializable]
public class LetterSocketGroup
{
    public LetterSocket[] letterSockets;
}
