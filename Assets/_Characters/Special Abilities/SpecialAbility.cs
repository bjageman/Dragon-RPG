using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core; 

namespace RPG.Characters{

	public struct AbilityUseParams{
		public IDamageable target;
		public float baseDamage;
		public GameObject parent;
		public Transform origin;

		public AbilityUseParams(IDamageable target, float baseDamage, GameObject parent, Transform origin){
			this.target = target;
			this.baseDamage = baseDamage;
			this.parent = parent;
			this.origin = origin;
		}


		public AbilityUseParams(Transform origin){
			this.target = null;
			this.baseDamage = 0;
			this.parent = null;
			this.origin = origin;
		}

	}

	public abstract class SpecialAbility : ScriptableObject {

		[Header("Special Ability General")]
		[SerializeField] float energyCost = 10f;
		[SerializeField] float attackRange = 10f;

		protected ISpecialAbility behavior;

		public float EnergyCost{ get { return this.energyCost; } }
		public float AttackRange{ get { return this.attackRange; } }

		abstract public void AddComponent(GameObject gameObjectToAttachTo);

		public void Use(AbilityUseParams parameters){
			behavior.Use(parameters);
		}
		
	}

	public interface ISpecialAbility{
		void Use(AbilityUseParams parameters);
	}
}
