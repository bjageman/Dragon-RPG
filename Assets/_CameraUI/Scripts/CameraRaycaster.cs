using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters;

namespace RPG.CameraUI
{
public class CameraRaycaster : MonoBehaviour
{
	[SerializeField] Texture2D walkCursor = null;
	[SerializeField] Texture2D enemyCursor = null;
	[SerializeField] Vector2 cursorHotspot = new Vector2 (0, 0); //

	const int WALKABLE_LAYER = 8;
    float maxRaycastDepth = 100f; // Hard coded value

	Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
	
	public delegate void OnMouseOverWalkable(Vector3 destination);
    public event OnMouseOverWalkable onMouseOverWalkable;

	public delegate void OnMouseOverEnemy(Enemy enemy);
    public event OnMouseOverEnemy onMouseOverEnemy;


    void Update()
        {
			screenRect = new Rect(0,0, Screen.width, Screen.height);
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
				//Implement UI Interaction
                return; // Stop looking for other objects
            }else{
				PerformRaycasts();
			}

        }

		void PerformRaycasts(){
			if (screenRect.Contains(Input.mousePosition)){
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				// Specify layer priorities
				if (RaycastForEnemy(ray)) { return; } 
				if (RaycastForWalkable(ray)){ return; }
			}
		}

        private bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
			bool hitStatus = Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
			if (hitStatus){
				var hitGameObject = hitInfo.collider.gameObject;
				var enemyHit = hitGameObject.GetComponent<Enemy>();
				if (enemyHit){
					Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
					onMouseOverEnemy(enemyHit);
					return true;
				}
			}
			
			return false;
        }

        private bool RaycastForWalkable(Ray ray)
        {
            
			RaycastHit hitInfo;
			LayerMask walkableLayer = 1 << WALKABLE_LAYER;
			bool hitStatus = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, walkableLayer);
			if (hitStatus){
				Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
				onMouseOverWalkable(hitInfo.point);
				return true;
			}
			return false;
        }
}
}