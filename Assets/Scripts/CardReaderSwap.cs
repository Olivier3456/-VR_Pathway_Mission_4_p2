using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardReaderSwap : XRSocketInteractor
{
    [Header("CardReader datas")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openDoorAudioClip;
    [SerializeField] private AudioClip dontOpenDoorAudioClip;
    [SerializeField] private GameObject doorLockingBar;

    //[SerializeField] private HandTrigger handTrigger;
    [SerializeField] private KeyCard keyCard;
    [SerializeField] private Transform attachPoint;
    [SerializeField] private Transform HandGrabbingAttachTransform;

    [SerializeField] private Material red_emissive_mat;
    [SerializeField] private Material red_mat;
    [SerializeField] private Material green_emissive_mat;
    [SerializeField] private Material green_mat;
    [SerializeField] private GameObject red_light;
    [SerializeField] private GameObject green_light;


    [HideInInspector] public bool CardInTrigger = false;

    private Vector3 originalHandGrabbingAttachTranformPosition;
    private Quaternion originanHandGrabbingAttachTransformRotation;

    private bool isHandGrabbingAttachTransformDetached = true;
    private bool cardJustEnterdedInTrigger = true;

    private void Update()
    {

        if (keyCard.HandGrabbing != null)
        {
            if (CardInTrigger)
            {              
                if (cardJustEnterdedInTrigger)
                {
                    originalHandGrabbingAttachTranformPosition = keyCard.HandGrabbing.attachTransform.localPosition;
                    originanHandGrabbingAttachTransformRotation = keyCard.HandGrabbing.attachTransform.localRotation;
                    cardJustEnterdedInTrigger = false;
                    isHandGrabbingAttachTransformDetached = false;                  

                }

                keyCard.HandGrabbing.attachTransform.position = new Vector3(attachPoint.position.x, keyCard.HandGrabbing.attachTransform.position.y, attachPoint.position.z);
                keyCard.HandGrabbing.attachTransform.rotation = attachPoint.rotation;
            }
            else if (!isHandGrabbingAttachTransformDetached)
            {
                isHandGrabbingAttachTransformDetached = true;
                cardJustEnterdedInTrigger = true;
                keyCard.HandGrabbing.attachTransform.localPosition = originalHandGrabbingAttachTranformPosition;
                keyCard.HandGrabbing.attachTransform.localRotation = originanHandGrabbingAttachTransformRotation;
            }
        }
    }
}
