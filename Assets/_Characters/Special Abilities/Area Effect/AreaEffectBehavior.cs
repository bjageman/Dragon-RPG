using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters {

public class AreaEffectBehavior : MonoBehaviour, ISpecialAbility {

	AreaEffectConfig config;

	public AreaEffectConfig Config{ set { this.config = value;}}

	public void Use(AbilityUseParams parameters){
		RaycastHit[] hits;
		// Static sphere cast for targets
		hits = Physics.SphereCastAll(transform.position, config.Radius, Vector3.up, config.Radius);
		foreach (RaycastHit hit in hits){
			var damageable = hit.collider.GetComponent<IDamageable>();
			if (damageable != null){
				damageable.TakeDamage(config.DamageToEachTarget);
			}
		}

	}
}

}