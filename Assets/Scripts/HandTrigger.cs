using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrigger : MonoBehaviour
{    
    [SerializeField] private CardReaderSwap cardReaderSwap;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyCard"))
        {
            cardReaderSwap.CardInTrigger = true;           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("KeyCard"))
        {
            cardReaderSwap.CardInTrigger = false;            
        }
    }
}