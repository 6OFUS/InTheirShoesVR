/*
* Author: Kevin Heng
* Date: 15/2/2025
* Description: Crossword Puzzle socket for pieces
* */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSocket : MonoBehaviour
{
    /// <summary>
    /// The letter that is stored in the socket after a letter block is placed in it.
    /// </summary>
    public char storedLetter;

    /// <summary>
    /// Called when a letter block enters the socket's trigger collider.
    /// It stores the letter of the block in the socket.
    /// </summary>
    /// <param name="other">The collider of the object entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Letter Block"))
        {
            LetterBlock block = other.GetComponent<LetterBlock>();
            storedLetter = block.letter;
            Debug.Log(storedLetter);
        }
    }

    /// <summary>
    /// Retrieves the letter stored in the socket.
    /// </summary>
    /// <returns>The letter stored in the socket.</returns>
    public char GetLetter()
    {
        return storedLetter;
    }
}

/// <summary>
/// A group of letter sockets in the crossword puzzle.
/// </summary>
[Serializable]
public class LetterSocketGroup
{
    public LetterSocket[] letterSockets;
}
