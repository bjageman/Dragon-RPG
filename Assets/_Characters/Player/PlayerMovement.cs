using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.AI;
using RPG.CameraUI; //TODO consider rewiring

namespace RPG.Characters
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (AICharacterControl))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster;
        Vector3 clickpoint;
        AICharacterControl aiCharacterControl = null;

        GameObject walkTarget;

        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aiCharacterControl = GetComponent<AICharacterControl>();
            
            walkTarget = new GameObject("Walk Target");

            cameraRaycaster.onMouseOverWalkable += OnMouseOverWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverWalkable(Vector3 destination){
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                aiCharacterControl.SetTarget(walkTarget.transform);
            }
        }

        void OnMouseOverEnemy(Enemy enemy){
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1)){
                aiCharacterControl.SetTarget(enemy.transform);
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
}
