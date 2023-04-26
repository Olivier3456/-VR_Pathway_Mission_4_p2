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
    [SerializeField] private AudioClip audioClip;

    private Material originalMat;
    private Renderer buttonRenderer;


    private void Start()
    {
        buttonRenderer = GetComponent<Renderer>();

       

        //pressedMat = Resources.Load<Material>("Materials/ForegroundColour2");
        originalMat = buttonRenderer.material;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        
        buttonRenderer.material = pressedMat;
        audioSource.Play();
    }


    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);

        buttonRenderer.material = originalMat;
    }



}
