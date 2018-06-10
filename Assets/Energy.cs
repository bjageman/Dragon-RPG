﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters{

	public class Energy : MonoBehaviour {

		[SerializeField] RawImage energyBar;
		[SerializeField] float maxEnergyPoints = 100f;
		[SerializeField] float pointsPerHit = 5f;

		[SerializeField] float currentEnergyPoints;
		float lastHitTime = 0f;

		CameraRaycaster cameraRaycaster;

		// Use this for initialization
		void Start () {
			currentEnergyPoints = maxEnergyPoints;
			RegisterRightMouseClick();
		}
		
		// Update is called once per frame
		void Update () {	
			float xValue = -(energyAsPercentage / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
  
		}

		private void RegisterRightMouseClick()
		{
			cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
			cameraRaycaster.notifyRightClickObservers += ProcessRightClick; //registering
		}

		private void ProcessRightClick(RaycastHit raycastHit, int layerHit){
			currentEnergyPoints = currentEnergyPoints - pointsPerHit;
			currentEnergyPoints = Mathf.Clamp(currentEnergyPoints, 0, maxEnergyPoints);
		}

		float energyAsPercentage{ get { return currentEnergyPoints / (float)maxEnergyPoints; } }
	}

}
