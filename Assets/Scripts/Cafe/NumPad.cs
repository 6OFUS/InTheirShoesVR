using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumPad : MonoBehaviour
{
    public TextMeshProUGUI queueNumInput;
    public string currentNum;

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
}
