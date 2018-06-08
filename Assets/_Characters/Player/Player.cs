using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.Core;
using RPG.CameraUI; //TODO consider rewiring
using RPG.Weapons; //TODO consider rewiring
using System;

namespace RPG.Characters
{
	public class Player : MonoBehaviour, IDamageable {

		[SerializeField] int enemyLayer = 9;
		[SerializeField] int maxHealthPoints = 100;
		[SerializeField] float currentHealthPoints;
		[SerializeField] int damagePerHit = 10;
		[SerializeField] float minTimeBetweenHits = .5f;
		[SerializeField] float maxHitRange = 2f;

		[SerializeField] Weapon weaponInUse;
		[SerializeField] AnimatorOverrideController animatorOverrideController;

		CameraRaycaster cameraRaycaster;
		float lastHitTime = 0f;

		public float healthAsPercentage{ get { return currentHealthPoints / (float)maxHealthPoints; } }

		public void Start()
        {
            SetCurrentMaxHealth();
            RegisterMouseClick();
            PutWeaponInHand();
            OverrideAnimatorController();
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void OverrideAnimatorController()
        {
            Animator animator = GetComponent<Animator>();
			animator.runtimeAnimatorController = animatorOverrideController;
			animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.getAnimClip();
        }

        private void PutWeaponInHand()
		{	
			var weaponPrefab = weaponInUse.GetWeaponPrefab();
			GameObject dominantHand = RequestDominantHand();
			var weapon = Instantiate(weaponPrefab, dominantHand.transform);
			weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
			weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
		}

		private GameObject RequestDominantHand(){
			var dominantHands = GetComponentsInChildren<DominantHand>();
			Assert.IsFalse(dominantHands.Length <= 0, "No DominantHand found on Player, please add one.");
			Assert.IsFalse(dominantHands.Length > 1, "Multiple dominant hand scripts on Player");
			return dominantHands[0].gameObject;
		}

		private void RegisterMouseClick()
		{
			cameraRaycaster = FindObjectOfType<CameraRaycaster>();
			cameraRaycaster.notifyMouseClickObservers += OnMouseClick; //registering
		}

		//TODO Make tidier
		void OnMouseClick(RaycastHit raycastHit, int layerHit){
			if (layerHit == enemyLayer){
				var enemy = raycastHit.collider.gameObject;
				// Check enemy is in range
				if ((enemy.transform.position - transform.position).magnitude > maxHitRange){ return; }
				var enemyComponent = enemy.GetComponent<Enemy>();
				if (Time.time - lastHitTime > minTimeBetweenHits){
					enemyComponent.TakeDamage(damagePerHit);
					lastHitTime = Time.time;
				}
				
			}  
		}

		public void TakeDamage(float damage)
		{
			currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
			if (currentHealthPoints <= 0){
				//Destroy(gameObject);
				print("player dead");
			}
		}
	}
}