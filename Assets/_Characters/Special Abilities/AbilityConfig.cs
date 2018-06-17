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
		[SerializeField] AudioClip audioClip;


		protected ISpecialAbility behavior;

		public float EnergyCost{ get { return this.energyCost; } }
		public float AttackRange{ get { return this.attackRange; } }
		public GameObject ParticlePrefab{ get { return this.particlePrefab; } }
		public AudioClip AudioClip { get { return this.audioClip; } }


		abstract public void AddComponent(GameObject gameObjectToAttachTo);

		public void Use(AbilityUseParams parameters){
			behavior.Use(parameters);
		}
		
	}

	public interface ISpecialAbility{
		void Use(AbilityUseParams parameters);
	}
}
