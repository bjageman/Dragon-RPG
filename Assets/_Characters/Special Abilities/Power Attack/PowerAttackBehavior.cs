using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{
	public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility {

		PowerAttackConfig config;

		public PowerAttackConfig Config{ set { this.config = value;}}

		public void Use(AbilityUseParams parameters){
			float damage = config.ExtraDamage + parameters.baseDamage;
			print(parameters.parent.name + " power attacked with " + damage);
			parameters.target.TakeDamage(damage);
			parameters.parent.GetComponent<Animator>().SetTrigger("Attack");
		}
	}
}