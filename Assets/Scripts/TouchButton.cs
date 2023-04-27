using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TouchButton : XRBaseInteractable
{
    [Header("Button Datas")]

    [SerializeField] private int _buttonValue;

    [SerializeField] private Material pressedMat;

    [SerializeField] private AudioSource audioSource;

    private NumberPad numberPad;

    private Material originalMat;
    private Renderer buttonRenderer;

    private XRBaseInteractor handOvering;



    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>();

        numberPad = GameObject.Find("Numberpad").GetComponent<NumberPad>();

        originalMat = buttonRenderer.material;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if (handOvering == null)
        {
            handOvering = args.interactorObject as XRBaseInteractor;
            buttonRenderer.material = pressedMat;
            audioSource.Play();
            numberPad.ButtonPressed(_buttonValue);
        }
    }


    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        if (handOvering != null && args.interactorObject as XRBaseInteractor == handOvering)
        {
            handOvering = null;

            buttonRenderer.material = originalMat;
        }
    }
}
