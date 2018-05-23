using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f; 
    [SerializeField] float attackMoveStopRadius = 5f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickpoint;
    bool isInGamepadMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)){ // G for gamepad. TODO allow mapping later
            isInGamepadMode = !isInGamepadMode;
            currentDestination = transform.position;
        }
        if (isInGamepadMode){
            ProcessGamepadMovement();
        }else{
            ProcessMouseMovement();
        }
        

    }
    
    private void ProcessGamepadMovement(){

        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        Vector3 cameraForward = Vector3.Scale(
            Camera.main.transform.forward, 
            new Vector3(1, 0, 1)
            ).normalized;
        Vector3 movement = v*cameraForward + h*Camera.main.transform.right;
        
        thirdPersonCharacter.Move(movement, false, false);

    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            clickpoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickpoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickpoint, attackMoveStopRadius);
                    break;
                default:
                    return;
            }
        }
        WalkToDestination();
    }

    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(currentDestination - transform.position, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening){
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickpoint, 0.2f);
        Gizmos.color = new Color(255f, 0f, 0, .5f);
        //Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

