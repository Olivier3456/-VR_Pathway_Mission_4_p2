using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    [Header("CardReader datas")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openDoorAudioClip;
    [SerializeField] private AudioClip dontOpenDoorAudioClip;
    [SerializeField] private GameObject doorLockingBar;

    [SerializeField] private Material red_emissive_mat;
    [SerializeField] private Material red_mat;
    [SerializeField] private Material green_emissive_mat;
    [SerializeField] private Material green_mat;
    [SerializeField] private GameObject red_light;
    [SerializeField] private GameObject green_light;

    bool doorOpen = false;

    private bool cardReadOnEntering;

    private float invalidValue = 0.97f;

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);

        cardReadOnEntering = VerifyCardMovementAndRotation(args);
    }
       

    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);

        //if (!doorOpen && cardReadOnEntering && VerifyCardMovementAndRotation(args))
        if (cardReadOnEntering && VerifyCardMovementAndRotation(args))
        {
            OpenDoor();
        }
        else if (!doorOpen)
        {
            DontOpenDoor();
        }
    }


    private void OpenDoor()
    {
        audioSource.PlayOneShot(openDoorAudioClip);
        doorLockingBar.SetActive(false);
        green_light.GetComponent<Renderer>().material = green_emissive_mat;
        doorOpen = true;
    }

    private void DontOpenDoor()
    {
        StartCoroutine(LightRed());
        audioSource.PlayOneShot(dontOpenDoorAudioClip);
    }


    private bool VerifyCardMovementAndRotation(BaseInteractionEventArgs args)
    {
        Vector3 cardMovementVector = args.interactableObject.transform.GetComponent<KeyCardMagneticTape>().GetKeyCardMovementVector();
        bool goodCardMovementVector = Vector3.Dot(cardMovementVector.normalized, Vector3.down) > invalidValue;

        float cardSpeed = cardMovementVector.magnitude / Time.deltaTime;
        bool goodCardSpeed = cardSpeed > 0.25f && cardSpeed < 1.2f;
        

        Vector3 cardForwardVector = args.interactableObject.transform.forward;
        bool goodCardForwardVector = Vector3.Dot(cardForwardVector, Vector3.down) < -invalidValue;

        Vector3 cardRightVector = args.interactableObject.transform.right;
        bool goodCardRightVector = Vector3.Dot(cardRightVector, Vector3.right) < -invalidValue;

        Vector3 cardUpVector = args.interactableObject.transform.up;
        bool goodCardUpVector = Vector3.Dot(cardUpVector, Vector3.forward) > invalidValue;


        Debug.Log("---------------------------------------------------------------------------");        
        Debug.Log("cardSpeed = " + cardSpeed);
        Debug.Log("Vector3.Dot(cardForwardVector, Vector3.down) = " + Vector3.Dot(cardForwardVector, Vector3.down));
        Debug.Log("Vector3.Dot(cardRightVector, Vector3.right) = " + Vector3.Dot(cardRightVector, Vector3.right));
        Debug.Log("Vector3.Dot(cardUpVector, Vector3.forward) = " + Vector3.Dot(cardUpVector, Vector3.forward));
        Debug.Log("---------------------------------------------------------------------------");

        if (goodCardMovementVector && goodCardForwardVector && goodCardRightVector && goodCardUpVector && goodCardSpeed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //private bool AreDirectionsAligned(Vector3 vector1, Vector3 vector2)
    //{
    //    return Vector3.Dot(vector1, vector2) < -invalidValue || Vector3.Dot(vector1, vector2) > invalidValue;   // Une seul sens nécessaire, retirer l'autre.
    //}

    IEnumerator LightRed()
    {
        for (int i = 0; i < 4; i++)
        {
            red_light.GetComponent<Renderer>().material = red_emissive_mat;
            yield return new WaitForSeconds(0.25f);
            red_light.GetComponent<Renderer>().material = red_mat;
            yield return new WaitForSeconds(0.25f);
        }
    }
}