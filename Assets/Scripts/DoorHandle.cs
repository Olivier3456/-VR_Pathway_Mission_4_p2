using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandle : XRBaseInteractable
{    
    [Header("DoorHandle datas")]
    [SerializeField] private GameObject _door;
    [SerializeField] float _openingDistance;    

    private Vector3 _doorClosedPosition;
    private XRBaseInteractor _handGrabbing;

    private Vector3 _handGrabbingPosition;
    private Vector3 _fromHandleToHandVector;


    [SerializeField] private GameObject _debugSphere;


    private void Start()
    {
        _doorClosedPosition = _door.transform.position;
    }


    private void Update()
    {
        if (_handGrabbing != null)
        {
            _handGrabbingPosition = _handGrabbing.transform.position;

            _fromHandleToHandVector = _handGrabbingPosition - transform.position;

            Vector3 movementToApplyAtTheDoor = Vector3.Project(_fromHandleToHandVector, transform.right);

            _door.transform.position += movementToApplyAtTheDoor;
            //_debugSphere.transform.position = transform.position + movementToApplyAtTheDoor;


        }
    }


 


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        Debug.Log("La poignée de la porte a été attrapée.");

        _handGrabbing = args.interactorObject as XRBaseInteractor;        
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        Debug.Log("La poignée de la porte a été lâchée.");
        _handGrabbing = null;
    }
}
