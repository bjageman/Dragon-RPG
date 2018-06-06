using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.AI;

[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] const int walkableLayerNumber = 8;
	[SerializeField] const int enemyLayerNumber = 9;

    ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickpoint;
    AICharacterControl aiCharacterControl = null;

    GameObject walkTarget;

    bool isInGamepadMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        aiCharacterControl = GetComponent<AICharacterControl>();
        
        walkTarget = new GameObject("Walk Target");
        currentDestination = transform.position;

        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
		cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick; //registering
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit){
        switch(layerHit){
            case enemyLayerNumber:
                //nav to enemy
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;
            default:
            Debug.LogWarning("Don't know how to handle mouse click for player movement");
                return;
        }
    }

    //TODO Make this get called again
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


}

