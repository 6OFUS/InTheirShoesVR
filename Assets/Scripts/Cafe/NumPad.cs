/*
* Author: Jennifer Lau
* Date: 15/2/2025
* Description: Cashier numpad script
* */
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumPad : MonoBehaviour
{
    /// <summary>
    /// Reference to the database for updating player and level data.
    /// </summary>
    Database database;

    /// <summary>
    /// The name of the current level.
    /// </summary>
    public string currentLevelName;

    /// <summary>
    /// The TextMeshProUGUI component that displays the current input number.
    /// </summary>
    public TextMeshProUGUI queueNumInput;

    /// <summary>
    /// The current number being input by the player.
    /// </summary>
    public string currentNum;

    /// <summary>
    /// An array of fruit sockets that hold fruit orders.
    /// </summary>
    public FruitSocket[] fruitSockets;

    /// <summary>
    /// The UI that appears when the level is complete.
    /// </summary>
    public GameObject levelCompleteUI;

    /// <summary>
    /// Controller responsible for handling messages.
    /// </summary>
    private MessagesController messagesController;


    /// <summary>
    /// The number of correct orders.
    /// </summary>
    private int correctOrders;

    /// <summary>
    /// The number of incorrect orders.
    /// </summary>
    private int incorrectOrders;

    /// <summary>
    /// Flag indicating if a correct order was found.
    /// </summary>
    private bool foundCorrectOrder;

    /// <summary>
    /// The points awarded for completing the level.
    /// </summary>
    private int levelPoints = 500;

    /// <summary>
    /// The points deducted for each incorrect order.
    /// </summary>
    private int pointsDeduction = 10;

    /// <summary>
    /// Adds a number to the current input when a button is pressed.
    /// </summary>
    /// <param name="num">The number pressed by the player.</param>
    public void InputNum(string num)
    {
        if(currentNum.Length < 6)
        {
            currentNum += num;
            queueNumInput.text = currentNum;
        }
    }

    /// <summary>
    /// Removes the last character from the current input when the undo button is pressed.
    /// </summary>
    public void Undo()
    {
        currentNum = currentNum.Substring(0, currentNum.Length - 1);
        queueNumInput.text = currentNum;
    }

    /// <summary>
    /// Handles the action when the player presses the Enter button to submit the order.
    /// </summary>
    public void Enter()
    {
        queueNumInput.text = "";
        foundCorrectOrder = false;
        foreach (var fruit in fruitSockets)
        {
            if(fruit.orderNum == currentNum && fruit.correct)
            {
                correctOrders++;
                Debug.Log("Correct fruit and order num");
                StartCoroutine(fruit.customer.ReturnToTable());
                Destroy(fruit.placedFruit);
                if(correctOrders  == fruitSockets.Length)
                {
                    levelCompleteUI.SetActive(true);
                    database.UpdateLevelComplete(GameManager.Instance.playerID, currentLevelName, true, "A5", DateTime.UtcNow.ToString("yyyy-MM-dd"), true);
                    database.UpdatePlayerPoints(GameManager.Instance.playerID, CalculatePoints());
                    StartCoroutine(messagesController.SendMultipleMessages(2, 3));
                }
                foundCorrectOrder = true;
                break;
            }
            else if (!foundCorrectOrder)
            {
                incorrectOrders++;
                StartCoroutine(fruit.customer.PointIdleCycle());
            }
        }

        if (!foundCorrectOrder)
        {
            incorrectOrders++;
        }
    }

    /// <summary>
    /// Calculates the total points earned by the player, factoring in deductions for incorrect orders.
    /// </summary>
    /// <returns>The player's updated point total.</returns>
    private int CalculatePoints()
    {
        levelPoints -= 10 * incorrectOrders;
        GameManager.Instance.playerPoints += levelPoints;
        Debug.Log(GameManager.Instance.playerPoints);
        return GameManager.Instance.playerPoints;
    }

    /// <summary>
    /// Called when the script is first initialized to set up database and message controller.
    /// </summary>
    private void Start()
    {
        database = FindObjectOfType<Database>();
        messagesController = FindObjectOfType<MessagesController>();
        StartCoroutine(messagesController.SendMultipleMessages(0, 1));
    }
}
