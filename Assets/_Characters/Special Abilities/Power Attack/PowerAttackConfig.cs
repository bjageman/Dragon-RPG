using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
	[CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
	public class PowerAttackConfig : AbilityConfig {

		[Header("Power Attack Specific")]
		[SerializeField] float extraDamage = 10f;

		public float ExtraDamage{ get { return this.extraDamage; } }

		public override void AddComponent(GameObject gameObjectToAttachTo){
			var behaviorComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehavior>();
			behaviorComponent.Config = this;
			behavior = behaviorComponent;
		}
	}
}