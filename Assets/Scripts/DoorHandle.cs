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


    Vector3 doorLastPosition;
    Vector3 doorLastMovement;

    private void Start()
    {
        _doorStartPosition = _door.transform.position;



        doorLastPosition = _door.transform.position;
        doorLastMovement = Vector3.zero;
    }


    private void Update()
    {
        if (_handGrabbing != null)
        {
            _fromHandleToHandVector = _handGrabbing.transform.position - transform.position;

            Vector3 forceToApplyToTheDoor = Vector3.Project(_fromHandleToHandVector, transform.right);

            //_debugSphere.transform.position = transform.position + forceToApplyToTheDoor;

            //Vector3 desiredDoorPosition = _door.transform.position + Vector3.Lerp(Vector3.zero, forceToApplyToTheDoor, Time.deltaTime);


            float lastMovementInertia = doorLastMovement.magnitude - (doorLastMovement.magnitude * (Time.deltaTime / inertia));

            Vector3 desiredDoorPosition = _door.transform.position +
                                          (doorLastMovement * lastMovementInertia) +
                                          Vector3.Lerp(Vector3.zero, forceToApplyToTheDoor, Time.deltaTime / inertia);







            float distanceFromDesiredPositionToStartPosition = Vector3.Distance(_doorStartPosition, desiredDoorPosition);

            if (distanceFromDesiredPositionToStartPosition <= _maxOpeningDistance)
            {
                //Pour que la porte soit ne puisse pas aller plus à gauche que son point de départ:
                if (_lastDistanceToStartPosition < distanceFromDesiredPositionToStartPosition
                    && Vector3.Dot(forceToApplyToTheDoor.normalized, transform.right) > 0)
                {
                    return;
                }

                _door.transform.position = desiredDoorPosition;

                _lastDistanceToStartPosition = distanceFromDesiredPositionToStartPosition;

                doorLastMovement = _door.transform.position - doorLastPosition;
                doorLastPosition = _door.transform.position;

            }
        }
        else if (doorLastMovement.magnitude > 0.001f)
        {

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
