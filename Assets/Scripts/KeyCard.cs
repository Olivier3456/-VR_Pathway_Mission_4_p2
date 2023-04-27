using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyCard : XRGrabInteractable
{
    private Rigidbody _rb;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(EjectOverSeconds(1.5f));        
    }

    public IEnumerator EjectOverSeconds(float seconds)
    {
        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.position += -transform.forward * Time.deltaTime * 0.1f;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        _rb.isKinematic = false;
    }
}
