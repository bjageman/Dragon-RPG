using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters{

	public class Energy : MonoBehaviour {

		[SerializeField] Image energyOrb = null;
		[SerializeField] float maxEnergyPoints = 100f;
		[SerializeField] float regenPoints = 1f;
		[SerializeField] float timeBetweenRegen = 1f;
		[SerializeField] float currentEnergyPoints;

		// Use this for initialization
		void Start () {
			currentEnergyPoints = maxEnergyPoints;
			StartCoroutine(RegenerateEnergy());
		}

        private IEnumerator RegenerateEnergy()
        {
			while(true){
				yield return new WaitForSeconds(timeBetweenRegen);
				AddEnergy(regenPoints);
			}
        }

        // TODO Will this work with simultaneous attacks?
        public bool IsEnergyAvailable(float amount){
			return amount <= currentEnergyPoints;
		}

		// TODO Combine these two functions
		public void AddEnergy(float energyPoints){
			currentEnergyPoints = currentEnergyPoints + energyPoints;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints, 0, maxEnergyPoints);
			UpdateEnergyBar();
		}

		public void ConsumeEnergy(float energyCost){
			currentEnergyPoints = currentEnergyPoints - energyCost;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints, 0, maxEnergyPoints);
			UpdateEnergyBar();
		}
		

        private void UpdateEnergyBar()
        {
            energyOrb.fillAmount = energyAsPercentage;
        }

        float energyAsPercentage{ get { return currentEnergyPoints / (float)maxEnergyPoints; } }
	}

}
