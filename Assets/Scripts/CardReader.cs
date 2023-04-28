using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    [Header("CardReader datas")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject doorLockingBar;

    [SerializeField] private Material red_emissive_mat;
    [SerializeField] private Material red_mat;
    [SerializeField] private Material green_emissive_mat;
    [SerializeField] private Material green_mat;
    [SerializeField] private GameObject red_light;
    [SerializeField] private GameObject green_light;

    bool doorOpen = false;

    private bool cardReadOnEntering;

    private float invalidValue = 0.9f;

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);

        cardReadOnEntering = VerifyCardMovementAndRotation(args);
    }


    protected override void OnHoverExiting(HoverExitEventArgs args)
    {
        base.OnHoverExiting(args);

        if (!doorOpen && cardReadOnEntering && VerifyCardMovementAndRotation(args))
        {
            OpenDoor();
        }
        else if (!doorOpen)
        {
            StartCoroutine(LightRed());
        }
    }


    private void OpenDoor()
    {
        audioSource.Play();
        doorLockingBar.SetActive(false);
        green_light.GetComponent<Renderer>().material = green_emissive_mat;
        doorOpen = true;
    }



    private bool VerifyCardMovementAndRotation(BaseInteractionEventArgs args)
    {
        Vector3 cardMovementVector = args.interactableObject.transform.GetComponent<KeyCardMagneticTape>().GetKeyCardMovementVector();
        bool goodCardMovementVector = Vector3.Dot(cardMovementVector.normalized, Vector3.down) > invalidValue;
        bool goodCardSpeed = cardMovementVector.magnitude > 0.0f && cardMovementVector.magnitude < 1000.0f;

        Vector3 cardForwardVector = args.interactableObject.transform.forward;
        bool goodCardForwardVector = AreDirectionsAligned(cardForwardVector, Vector3.down);

        Vector3 cardRightVector = args.interactableObject.transform.right;
        bool goodCardRightVector = AreDirectionsAligned(cardRightVector, Vector3.right);

        Vector3 cardUpVector = args.interactableObject.transform.up;
        bool goodCardUpVector = AreDirectionsAligned(cardUpVector, Vector3.forward);


        Debug.Log("---------------------------------------------------------------------------");
        // Debug.Log("Vector3.Dot : " + Vector3.Dot(cardMovementVector.normalized, Vector3.down));
        Debug.Log("cardMovementVector.magnitude = " + cardMovementVector.magnitude);
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


    private bool AreDirectionsAligned(Vector3 vector1, Vector3 vector2)
    {
        return Vector3.Dot(vector1, vector2) < -invalidValue || Vector3.Dot(vector1, vector2) > invalidValue;
    }

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
