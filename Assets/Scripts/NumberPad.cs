using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPad : MonoBehaviour
{
    [SerializeField] private int[] goodCode;

    private int[] codeEntered;

    private int index = 0;


    void Start()
    {
        codeEntered = new int[goodCode.Length];
    }


    public void ButtonPressed(int buttonValue)
    {
        codeEntered[index] = buttonValue;
        index++;      
        Debug.Log(buttonValue);

        if (index == goodCode.Length)
        {
            if (CheckCode())
            {
                GoodCode();
            }
            else
            {
                WrongCode();
            }
        }
        
        
    }

    bool CheckCode()
    {      
        for (int i = 0; i < goodCode.Length; i++)
        {
            if (goodCode[i] != codeEntered[i])
            {
                return false;
            }
        }
        return true;
    }



    void GoodCode()
    {
        Debug.Log("Bon code !");
    }

    void WrongCode()
    {
        index = 0;
        Debug.Log("Mauvais code !");
    }

}
