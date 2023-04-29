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
    private bool isHandGrabbingAttachTransformDetached = true;
    private bool cardJustEnterdedInTrigger = true;

    private Vector3 originalHandGrabbingAttachTranformPosition;
    private Quaternion originanHandGrabbingAttachTransformRotation;

    private bool doorOpen = false;
    private bool canSnapAgain = true;



    private void Update()
    {

        if (keyCard.HandGrabbing != null)
        {
            if (CardInTrigger)
            {
                if (canSnapAgain)
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
                
            }
            else if (!isHandGrabbingAttachTransformDetached)
            {
                isHandGrabbingAttachTransformDetached = true;
                cardJustEnterdedInTrigger = true;

                float cardSpeed = keyCard.MovementVector.magnitude / Time.deltaTime;
                bool goodCardSpeed = cardSpeed > 0.25f && cardSpeed < 1.2f;

                if (goodCardSpeed && !doorOpen)
                {
                    OpenDoor();
                }
                else if (!doorOpen)
                {
                    DontOpenDoor();
                }


                keyCard.HandGrabbing.attachTransform.localPosition = originalHandGrabbingAttachTranformPosition;
                keyCard.HandGrabbing.attachTransform.localRotation = originanHandGrabbingAttachTransformRotation;

                StartCoroutine(CardSnapAgainDelay());
            }
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

    IEnumerator CardSnapAgainDelay()
    {
        canSnapAgain = false;
        yield return new WaitForSeconds(0.5f);
        canSnapAgain = true;
    }
}