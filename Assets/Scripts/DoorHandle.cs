using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// Version avec coroutines (la version sans coroutines est plus bas, en commentaire).
public class DoorHandle : XRBaseInteractable
{
    [Header("DoorHandle datas")]
    [SerializeField] private GameObject _door;
    [SerializeField] private float _maxOpeningDistance = 0.9f;
    [SerializeField][Range(0, 1)] private float _reboundStrength = 0.5f;
    public bool DoorCanOpen = false;

    private XRBaseInteractor _handGrabbing;
    private Vector3 _lastDoorMovement = Vector3.zero;
    private Vector3 _movementMiddlePoint;

    private void Start()
    {
        _maxOpeningDistance *= 0.5f;
        _movementMiddlePoint = _door.transform.position + (-transform.right * _maxOpeningDistance);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _handGrabbing = args.interactorObject as XRBaseInteractor;

        if (DoorCanOpen) StartCoroutine(HandleGrabbed());
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        _handGrabbing = null;

        if (DoorCanOpen) StartCoroutine(HandleNotGrabbed());
    }

    IEnumerator HandleGrabbed()
    {
        if (_handGrabbing == null) yield break;
        Vector3 _fromHandleToHandVector = _handGrabbing.transform.position - transform.position;
        Vector3 _forceToApplyToTheDoor = Vector3.Project(_fromHandleToHandVector, transform.right);
        MoveDoor(_forceToApplyToTheDoor);
        yield return null;
        StartCoroutine(HandleGrabbed());
    }

    IEnumerator HandleNotGrabbed()
    {
        if (_handGrabbing != null) yield break;
        MoveDoor(Vector3.zero);
        yield return null;
        if (_lastDoorMovement.magnitude > 0.01f * Time.deltaTime) StartCoroutine(HandleNotGrabbed());
    }

    private void MoveDoor(Vector3 forceToApply)
    {
        // Inertia + force applied to the door by the hand of the player:
        Vector3 desiredDoorMovement = _lastDoorMovement * (1 - (Time.deltaTime / 0.5f)) + Vector3.Lerp(Vector3.zero, forceToApply, Time.deltaTime * 0.03f);
        Vector3 desiredDoorPosition = _door.transform.position + desiredDoorMovement;
        float distanceFromDesiredPositionToMovementMiddlePoint = Vector3.Distance(_movementMiddlePoint, desiredDoorPosition);

        if (distanceFromDesiredPositionToMovementMiddlePoint > _maxOpeningDistance)
        {
            _lastDoorMovement = -_lastDoorMovement * _reboundStrength;
        }
        else
        {
            _door.transform.position = desiredDoorPosition;
            _lastDoorMovement = desiredDoorMovement;
        }
    }
}








//public class DoorHandle : XRBaseInteractable
//{
//    [Header("DoorHandle datas")]
//    [SerializeField] private GameObject _door;    
//    [SerializeField] private float _maxOpeningDistance = 0.9f;
//    [SerializeField][Range(0, 1)] private float _reboundStrength = 0.5f;
//    public bool DoorCanOpen = false;

//    private XRBaseInteractor _handGrabbing;
//    private Vector3 _lastDoorMovement = Vector3.zero;
//    private Vector3 _movementMiddlePoint;    

//    private void Start()
//    {        
//        _maxOpeningDistance *= 0.5f;
//        _movementMiddlePoint = _door.transform.position + (-transform.right * _maxOpeningDistance);        
//    }


//    private void Update()
//    {
//        if (_handGrabbing != null && DoorCanOpen)
//        {
//            Vector3 _fromHandleToHandVector = _handGrabbing.transform.position - transform.position;
//            Vector3 _forceToApplyToTheDoor = Vector3.Project(_fromHandleToHandVector, transform.right);
//            MoveDoor(_forceToApplyToTheDoor);
//        }
//        else if (_lastDoorMovement.magnitude > 0.01f * Time.deltaTime)   // To apply inertia even when the handle is not grabbed.
//        {
//            MoveDoor(Vector3.zero);
//        }
//    }


//    private void MoveDoor(Vector3 forceToApply)
//    {
//        // Inertia + force applied to the door by the hand of the player:
//        Vector3 desiredDoorMovement = _lastDoorMovement * (1 - (Time.deltaTime / 0.5f)) + Vector3.Lerp(Vector3.zero, forceToApply, Time.deltaTime * 0.03f);
//        Vector3 desiredDoorPosition = _door.transform.position + desiredDoorMovement;
//        float distanceFromDesiredPositionToMovementMiddlePoint = Vector3.Distance(_movementMiddlePoint, desiredDoorPosition);

//        if (distanceFromDesiredPositionToMovementMiddlePoint > _maxOpeningDistance)
//        {
//            _lastDoorMovement = -_lastDoorMovement * _reboundStrength;
//        }
//        else
//        {
//            _door.transform.position = desiredDoorPosition;
//            _lastDoorMovement = desiredDoorMovement;
//        }
//    }


//    protected override void OnSelectEntered(SelectEnterEventArgs args)
//    {
//        base.OnSelectEntered(args);
//        _handGrabbing = args.interactorObject as XRBaseInteractor;
//    }


//    protected override void OnSelectExited(SelectExitEventArgs args)
//    {
//        base.OnSelectExited(args);
//        _handGrabbing = null;
//    }
//}