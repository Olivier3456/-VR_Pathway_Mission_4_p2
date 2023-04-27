using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReader : XRSocketInteractor
{
    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);

        Vector3 cardMovementVector = args.interactableObject.transform.GetComponent<KeyCard>().MovementVector;        

        // Debug.Log("Vector3.Dot : " + Vector3.Dot(cardMovementVector.normalized, Vector3.down));

        if (Vector3.Dot(cardMovementVector.normalized, Vector3.down) > 0.9f)
        {
            Debug.Log("La carte a été passée dans le bon sens !");
        }
    }
}
