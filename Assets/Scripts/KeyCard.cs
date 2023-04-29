using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyCard : XRGrabInteractable
{
    private Rigidbody _rb;

    private Vector3 _lastPosition;

    private Vector3 _movementVector;
    public Vector3 MovementVector { get { return _movementVector; } }


    private XRBaseInteractor _handGrabbing;
    public XRBaseInteractor HandGrabbing { get { return _handGrabbing; } }


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _lastPosition = transform.position;
    }

    private void Update()
    {
        _movementVector = transform.position - _lastPosition;

        _lastPosition = transform.position;
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




    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _rb.isKinematic = false;

        _handGrabbing = args.interactorObject as XRBaseInteractor;
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        _rb.isKinematic = false;
    }
}
