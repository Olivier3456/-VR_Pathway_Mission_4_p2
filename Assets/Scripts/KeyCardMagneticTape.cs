using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyCardMagneticTape : XRBaseInteractable
{
    [Header("KeyCardMagneticTapeDatas")]
    [SerializeField] private KeyCard keyCard;

    public Vector3 GetKeyCardMovementVector()
    {
        return keyCard.MovementVector;
    }
}
