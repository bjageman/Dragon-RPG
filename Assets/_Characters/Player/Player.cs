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

		[SerializeField] int maxHealthPoints = 100;
		[SerializeField] float currentHealthPoints;
		[SerializeField] int damagePerHit = 10;
		[SerializeField] float minTimeBetweenHits = .5f;
		[SerializeField] float maxHitRange = 2f;

		[SerializeField] Weapon weaponInUse = null;
		[SerializeField] AnimatorOverrideController animatorOverrideController = null;

		Animator animator;
		CameraRaycaster cameraRaycaster;
		float lastHitTime = 0f;

		public float healthAsPercentage{ get { return currentHealthPoints / (float)maxHealthPoints; } }

		public void Start()
        {
            SetCurrentMaxHealth();
            RegisterMouseClick();
            PutWeaponInHand();
            SetupRuntimeAnimator();
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
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
			cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
			cameraRaycaster.onMouseOverEnemy += onMouseOverEnemy; //registering
		}

		void onMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy))
                {
                    AttackTarget(enemy);
                }
        }

        private void AttackTarget(Enemy enemy)
        {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack"); // TODO make const
				gameObject.transform.LookAt(enemy.transform);
                enemy.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(Enemy enemy)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
}

		public void TakeDamage(float damage)
		{
			currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
			if (currentHealthPoints <= 0){
				//Destroy(gameObject);
			}
		}
	}
}