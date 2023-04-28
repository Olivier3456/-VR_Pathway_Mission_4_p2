using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    [Header("CardReader datas")]
    [SerializeField] private AudioSource audioSource;

    private bool cardRead;

    private float invalidValue = 0.9f;

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        cardRead = false;

        base.OnHoverEntering(args);

        Vector3 cardMovementVector = args.interactableObject.transform.GetComponent<KeyCardMagneticTape>().GetKeyCardMovementVector();
        bool goodCardMovementVector = Vector3.Dot(cardMovementVector.normalized, Vector3.down) > invalidValue;

        Vector3 cardForwardVector = args.interactableObject.transform.forward;
        bool goodCardForwardVector = Vector3.Dot(cardForwardVector, Vector3.down) < -invalidValue
                                  || Vector3.Dot(cardForwardVector, Vector3.down) > invalidValue;

        Vector3 cardRightVector = args.interactableObject.transform.right;
        bool goodCardRightVector = Vector3.Dot(cardRightVector, Vector3.right) < -invalidValue
                                || Vector3.Dot(cardRightVector, Vector3.right) > invalidValue;

        Vector3 cardUpVector = args.interactableObject.transform.up;
        bool goodCardUpVector = Vector3.Dot(cardUpVector, Vector3.forward) < -invalidValue
                             || Vector3.Dot(cardUpVector, Vector3.forward) > invalidValue;



        // Debug.Log("Vector3.Dot : " + Vector3.Dot(cardMovementVector.normalized, Vector3.down));
        Debug.Log("Vector3.Dot(cardForwardVector, Vector3.down) = " + Vector3.Dot(cardForwardVector, Vector3.down));
        Debug.Log("Vector3.Dot(cardRightVector, Vector3.right) = " + Vector3.Dot(cardRightVector, Vector3.right));
        Debug.Log("Vector3.Dot(cardUpVector, Vector3.forward) = " + Vector3.Dot(cardUpVector, Vector3.forward));

        if (goodCardMovementVector && goodCardForwardVector && goodCardRightVector && goodCardUpVector)
        {            
            audioSource.Play();
            cardRead = true;
        }
    }


    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);


    }
}
