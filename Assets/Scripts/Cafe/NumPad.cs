using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumPad : MonoBehaviour
{
    Database database;
    public string currentLevelName;
    public TextMeshProUGUI queueNumInput;
    public string currentNum;
    public FruitSocket[] fruitSockets;
    public GameObject levelCompleteUI;

    private int correctOrders;

    private int incorrectOrders;
    private bool foundCorrectOrder;

    private int levelPoints = 500;
    private int pointsDeduction = 10;

    public void InputNum(string num)
    {
        if(currentNum.Length < 6)
        {
            currentNum += num;
            queueNumInput.text = currentNum;
        }
    }

    public void Undo()
    {
        currentNum = currentNum.Substring(0, currentNum.Length - 1);
        queueNumInput.text = currentNum;
    }

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

    private int CalculatePoints()
    {
        levelPoints -= 10 * incorrectOrders;
        GameManager.Instance.playerPoints += levelPoints;
        Debug.Log(GameManager.Instance.playerPoints);
        return GameManager.Instance.playerPoints;
    }

    private void Start()
    {
        database = FindObjectOfType<Database>();
    }
}
