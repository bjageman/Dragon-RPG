using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core; 

namespace RPG.Characters{

	public struct AbilityUseParams{
		public IDamageable target;
		public float baseDamage;
		public Transform origin;

		public AbilityUseParams(IDamageable target, float baseDamage,Transform origin){
			this.target = target;
			this.baseDamage = baseDamage;
			this.origin = origin;
		}

	}

	public abstract class AbilityConfig : ScriptableObject {

		[Header("Special Ability General")]
		[SerializeField] float energyCost = 10f;
		[SerializeField] float attackRange = 10f;
		[SerializeField] GameObject particlePrefab = null;
		[SerializeField] AudioClip[] audioClips;

		protected AbilityBehavior behavior;

		public abstract AbilityBehavior GetBehaviorComponent(GameObject objectToAttachTo);

		public float EnergyCost{ get { return this.energyCost; } }
		public float AttackRange{ get { return this.attackRange; } }
		public GameObject ParticlePrefab{ get { return this.particlePrefab; } }
		public AudioClip[] AudioClips { get { return this.audioClips; } }

		public void AttachAbilityTo(GameObject objectToAttachTo){
			AbilityBehavior behaviorComponent = GetBehaviorComponent(objectToAttachTo);
			behaviorComponent.Config = this;
			behavior = behaviorComponent;
		}

		public void Use(AbilityUseParams parameters){
			behavior.Use(parameters);
		}

		public AudioClip GetRandomSound(){
			if (audioClips.Length > 0){
				return audioClips[Random.Range(0, audioClips.Length - 1)];
			}
			return null;
		}
		
	}

}
