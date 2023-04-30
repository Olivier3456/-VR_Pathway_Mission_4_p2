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
    [SerializeField] private DoorHandle doorHandle;
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
    private bool cardJustEnteredInTrigger = true;

    private Vector3 originalHandGrabbingAttachTranformPosition;
    private Quaternion originanHandGrabbingAttachTransformRotation;

    private bool doorOpen = false;

    private bool canSnapAgain = true;

    private float cardYpositionWhenEnteringTrigger;


    private void Update()
    {
        if (keyCard.HandGrabbing != null)
        {
            if (CardInTrigger && canSnapAgain)
            {
                if (cardJustEnteredInTrigger)
                {
                    originalHandGrabbingAttachTranformPosition = keyCard.HandGrabbing.attachTransform.localPosition;
                    originanHandGrabbingAttachTransformRotation = keyCard.HandGrabbing.attachTransform.localRotation;

                    cardYpositionWhenEnteringTrigger = keyCard.transform.position.y;

                    cardJustEnteredInTrigger = false;
                    isHandGrabbingAttachTransformDetached = false;
                }

                keyCard.HandGrabbing.attachTransform.position = new Vector3(attachPoint.position.x, keyCard.HandGrabbing.attachTransform.position.y, attachPoint.position.z);
                keyCard.HandGrabbing.attachTransform.rotation = attachPoint.rotation;
            }
            else if (!isHandGrabbingAttachTransformDetached && canSnapAgain)
            {
                isHandGrabbingAttachTransformDetached = true;
                cardJustEnteredInTrigger = true;

                float cardSpeed = keyCard.MovementVector.magnitude / Time.deltaTime;
                bool goodCardSpeed = cardSpeed > 0.5f && cardSpeed < 2.2f;
                Debug.Log("cardSpeed = " + cardSpeed);

                if (goodCardSpeed && !doorOpen && cardYpositionWhenEnteringTrigger > keyCard.transform.position.y + 0.35f)
                {
                    OpenDoor();
                }
                else if (!doorOpen && canSnapAgain && Mathf.Abs(cardYpositionWhenEnteringTrigger - keyCard.transform.position.y) > 0.1f)
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
        doorHandle.DoorCanOpen = true;
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