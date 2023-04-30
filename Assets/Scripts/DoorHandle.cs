using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandle : XRBaseInteractable
{
    [Header("DoorHandle datas")]
    [SerializeField] private GameObject _door;    
    [SerializeField] float _maxOpeningDistance = 0.9f;

    private Vector3 _doorStartPosition;
    private XRBaseInteractor _handGrabbing;

    private Vector3 _fromHandleToHandVector;

    private float _lastDistanceToStartPosition = 0;

    [SerializeField] private GameObject _debugSphere;

    [SerializeField] private float inertia = 10;

    private float _distanceFromDesiredPositionToStartPosition = 0;

    private Vector3 _forceToApplyToTheDoor;

    public bool DoorCanOpen = false;


    Vector3 _doorLastPosition;
    Vector3 _doorLastMovement;

    private void Start()
    {
        _doorStartPosition = _door.transform.position;

        _forceToApplyToTheDoor = Vector3.zero;

        _doorLastPosition = _door.transform.position;
        _doorLastMovement = Vector3.zero;
    }


    private void Update()
    {
        if (_handGrabbing != null && DoorCanOpen)
        {
            _fromHandleToHandVector = _handGrabbing.transform.position - transform.position;

            _forceToApplyToTheDoor = Vector3.Project(_fromHandleToHandVector, transform.right);                 

            MoveDoor(_forceToApplyToTheDoor);            
        }
        else if (_doorLastMovement.magnitude > 0.1f * Time.deltaTime)
        {
            MoveDoor(Vector3.zero);
        }
    }



    private void MoveDoor(Vector3 forceToApply)
    {
        Vector3 desiredDoorPosition;

        if (forceToApply == Vector3.zero)
        {
            Vector3 lastMovementInertia = _doorLastMovement * (1 - (Time.deltaTime / inertia));
            desiredDoorPosition = _door.transform.position + lastMovementInertia;           
        }
        else
        {
            desiredDoorPosition = _door.transform.position + Vector3.Lerp(Vector3.zero, _forceToApplyToTheDoor, Time.deltaTime);
        }  

        _distanceFromDesiredPositionToStartPosition = Vector3.Distance(_doorStartPosition, desiredDoorPosition);

        if (_distanceFromDesiredPositionToStartPosition <= _maxOpeningDistance)
        {
            //Pour que la porte soit ne puisse pas aller plus à gauche que son point de départ :
            if (_lastDistanceToStartPosition < _distanceFromDesiredPositionToStartPosition
                && Vector3.Dot(_forceToApplyToTheDoor.normalized, transform.right) > 0)
            {
                return;
            }

            _door.transform.position = desiredDoorPosition;

            _lastDistanceToStartPosition = _distanceFromDesiredPositionToStartPosition;

            _doorLastMovement = _door.transform.position - _doorLastPosition;
            _doorLastPosition = _door.transform.position;
        }
    }



    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _handGrabbing = args.interactorObject as XRBaseInteractor;
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _handGrabbing = null;
    }
}