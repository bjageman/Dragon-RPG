using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
	[CreateAssetMenu(menuName = "RPG/Special Ability/Self Heal")]
	public class SelfHealConfig : AbilityConfig {

		[Header("Self Heal Config")]
		[SerializeField] float healAmount = 10f;

		public float HealAmount { get { return this.healAmount; } }

		public override AbilityBehavior GetBehaviorComponent(GameObject objectToAttachTo){
			return objectToAttachTo.AddComponent<SelfHealBehavior>();
		}

	}
}