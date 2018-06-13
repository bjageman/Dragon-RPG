using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters{

	public class Energy : MonoBehaviour {

		[SerializeField] RawImage energyBar = null;
		[SerializeField] float maxEnergyPoints = 100f;

		[SerializeField] float currentEnergyPoints;

		// Use this for initialization
		void Start () {
			currentEnergyPoints = maxEnergyPoints;
		}

		// TODO Will this work with simultaneous attacks?
		public bool IsEnergyAvailable(float amount){
			return amount < currentEnergyPoints;
		}

		public void ConsumeEnergy(float energyCost){
			currentEnergyPoints = currentEnergyPoints - energyCost;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints, 0, maxEnergyPoints);
			UpdateEnergyBar();
		}
		

        private void UpdateEnergyBar()
        {
            float xValue = -(energyAsPercentage / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float energyAsPercentage{ get { return currentEnergyPoints / (float)maxEnergyPoints; } }
	}

}
