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


    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>();

        numberPad = GameObject.Find("Numberpad").GetComponent<NumberPad>();
       
        originalMat = buttonRenderer.material;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        
        buttonRenderer.material = pressedMat;
        audioSource?.Play();
        numberPad.ButtonPressed(_buttonValue);
    }


    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        buttonRenderer.material = originalMat;
    }
}
