using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.Core;
using RPG.CameraUI; //TODO consider rewiring
using RPG.Weapons; //TODO consider rewiring
using System;

namespace RPG.Characters
{
	public class Player : MonoBehaviour, IDamageable {

		[Header("Health")]
		[SerializeField] int maxHealthPoints = 100;
		[SerializeField] float currentHealthPoints = 0f;
		[Header("Damage")]
		[SerializeField] int baseDamage = 10;
		[SerializeField] Weapon weaponInUse = null;
		[Range (.1f, 1.0f)] [SerializeField] float criticalHitChance = 0.1f;
		[SerializeField] float criticalHitMultiplier = 1.25f;
		[SerializeField] ParticleSystem criticalHitParticleSystem = null;
		[Header("Animation")]
		[SerializeField] AnimatorOverrideController animatorOverrideController = null;
		[Header("Sounds")]
		[SerializeField] AudioClip[] damageSounds = new AudioClip[0];
		[SerializeField] AudioClip[] deathSounds = new AudioClip[0];
 
		// Temporarily serialized for dubbing
		[Header("Special Abilities")]
		[SerializeField] AbilityConfig[] abilities;
		
		Enemy enemy = null;
		AudioSource audioSource;
		Animator animator;
		CameraRaycaster cameraRaycaster;
		float lastHitTime = 0f;

		public float healthAsPercentage { get { return currentHealthPoints / (float)maxHealthPoints; } }

		public float CurrentHealthPoints { 
			get { return this.currentHealthPoints; } 
			set { this.currentHealthPoints = value; } 
			}

		public void Start()
        {
            SetCurrentMaxHealth();
            RegisterMouseClick();
            PutWeaponInHand();
            SetupRuntimeAnimator();
			SetupAudio();
            AddAbilities();
        }

		public void Update(){
			if (healthAsPercentage > Mathf.Epsilon){
				ScanForAbilityKeyDown();
			}
		}

		// TODO Still hits enemy when not selected
        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 0; keyIndex < abilities.Length; keyIndex++){
				if (Input.GetKeyDown(keyIndex.ToString())){
					AttemptSpecialAbility(keyIndex);
				}
			}

        }

        private void AddAbilities()
        {
            for (int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++){
				abilities[abilityIndex].AddComponent(gameObject);
			}
			
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

		void onMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
			if (Input.GetMouseButton(0) && IsTargetInRange(weaponInUse.GetMaxAttackRange()))
                {
                    AttackTarget();
                }
			else if (Input.GetMouseButtonDown(1)){
				AttemptSpecialAbility(0);
			}
        }

		private void AttemptSpecialAbility(int abilityIndex)
        {
			Energy energy = GetComponent<Energy>();
			float energyCost = abilities[abilityIndex].EnergyCost;
			float attackRange = abilities[abilityIndex].AttackRange;

			if (energy.IsEnergyAvailable(energyCost) && IsTargetInRange(attackRange)){
				energy.ConsumeEnergy(energyCost);
				var abilityParams = new AbilityUseParams(enemy, baseDamage  + weaponInUse.AdditionalDamage, enemy.transform);
				abilities[abilityIndex].Use(abilityParams);
			}
		}

        private void AttackTarget()
        {
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits())
            {
                animator.SetTrigger("Attack"); // TODO make const
                gameObject.transform.LookAt(enemy.transform);
                enemy.TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
			bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
			float damage = baseDamage + weaponInUse.AdditionalDamage;
            if (isCriticalHit){
				criticalHitParticleSystem.Play();
				damage = damage * criticalHitMultiplier;
			}
			return damage;
        }

        private bool IsTargetInRange(float maxAttackRange)
        {
            if (!enemy) { return false; } 
			float distanceToTarget = (enemy.transform.position - transform.position).magnitude;
            return distanceToTarget <= maxAttackRange;
		}

		public void TakeDamage(float damage)
		{
			currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
			if (currentHealthPoints > 0){
				audioSource.clip = damageSounds[UnityEngine.Random.Range(0, damageSounds.Length - 1)];
				audioSource.Play();
			}else{ 
				StartCoroutine(KillPlayer()); 
				}
        }

		public void Heal(float points){
			currentHealthPoints = Mathf.Clamp(currentHealthPoints + points, 0f, maxHealthPoints);
		}

        private IEnumerator KillPlayer()
        {			
            animator.SetTrigger("Death");
			float soundLength = PlayDeathSound();
			yield return new WaitForSecondsRealtime(soundLength);

            //TODO Replace with 'Game Over' menu and reload option
            int sceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneBuildIndex);
        }


        private float PlayDeathSound()
        {
			var rnd = UnityEngine.Random.Range(0, deathSounds.Length - 1);
            AudioClip deathSound = deathSounds[rnd];
			audioSource.clip = deathSound;
			audioSource.Play();
			return audioSource.clip.length;
        }

        
    }
}