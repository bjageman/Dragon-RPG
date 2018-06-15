using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.Core;
using RPG.CameraUI; //TODO consider rewiring
using RPG.Weapons; //TODO consider rewiring

namespace RPG.Characters
{
	public class Player : MonoBehaviour, IDamageable {

		[SerializeField] int maxHealthPoints = 100;
		[SerializeField] float currentHealthPoints;
		[SerializeField] int baseDamage = 10;
		[SerializeField] Weapon weaponInUse = null;
		[SerializeField] AnimatorOverrideController animatorOverrideController = null;
		// Temporarily serialized for dubbing
		[SerializeField] SpecialAbility[] abilities;

		[SerializeField] AudioClip[] damageSounds = new AudioClip[0];
		[SerializeField] AudioClip[] deathSounds = new AudioClip[0];
		
		bool isAlive = true;
		AudioSource audioSource;
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
			SetupAudio();
            AddAbilities();
        }

        private void AddAbilities()
        {
            abilities[0].AddComponent(gameObject);
        }

        private void SetupAudio()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
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
            if (Input.GetMouseButton(0) && IsTargetInRange(enemy, weaponInUse.GetMaxAttackRange()))
                {
                    AttackTarget(enemy);
                }
			else if (Input.GetMouseButtonDown(1)){
				AttemptSpecialAbility(0, enemy);
			}
        }

		private void AttemptSpecialAbility(int abilityIndex, Enemy enemy)
        {
			Energy energy = GetComponent<Energy>();
			float energyCost = abilities[abilityIndex].EnergyCost;
			float attackRange = abilities[abilityIndex].AttackRange;

			if (energy.IsEnergyAvailable(energyCost) && IsTargetInRange(enemy, attackRange)){
				energy.ConsumeEnergy(energyCost);
				var abilityParams = new AbilityUseParams(enemy, baseDamage, gameObject, enemy.transform);
				abilities[abilityIndex].Use(abilityParams);
			}
		}

        private void AttackTarget(Enemy enemy)
        {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
				print("Attack");
                animator.SetTrigger("Attack"); // TODO make const
				gameObject.transform.LookAt(enemy.transform);
                enemy.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(Enemy enemy, float maxAttackRange)
        {
            float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= maxAttackRange;
}

		public void TakeDamage(float damage)
		{
			if (isAlive) {
				ReduceHealth(damage);
				if (currentHealthPoints <= 0)
				{
					StartCoroutine(KillPlayer());
				}else{
					PlayDamageSound();
				}
			}
			
        }

        private IEnumerator KillPlayer()
        {
            isAlive = false;
			float soundLength = PlayDeathSound();
            Debug.Log("Death Animation");
            //TODO Replace with 'Game Over' menu and reload option
            yield return new WaitForSeconds(soundLength);
            int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneBuildIndex);
        }

		private void PlayDamageSound()
        {
			var rnd = Random.Range(0, damageSounds.Length - 1);
            AudioClip damageSound = damageSounds[rnd];
			audioSource.clip = damageSound;
			audioSource.Play();
        }

        private float PlayDeathSound()
        {
			var rnd = Random.Range(0, deathSounds.Length - 1);
            AudioClip deathSound = deathSounds[rnd];
			audioSource.clip = deathSound;
			audioSource.Play();
			return audioSource.clip.length;
        }

        private void ReduceHealth(float damage)
        {
			currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }
    }
}