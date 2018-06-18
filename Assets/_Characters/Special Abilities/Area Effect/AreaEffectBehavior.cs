using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {

public class AreaEffectBehavior : AbilityBehavior {

	public override void Use(AbilityUseParams parameters)
        {
            DealRadialDamage();
            PlayAbilitySound();
			PlayParticleEffect();
        }

    private void DealRadialDamage()
    {
        RaycastHit[] hits;
        // Static sphere cast for targets
        hits = Physics.SphereCastAll(transform.position, (config as AreaEffectConfig).Radius, Vector3.up, (config as AreaEffectConfig).Radius);
        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.GetComponent<IDamageable>();
            bool hitPlayer = hit.collider.GetComponent<Player>();
            if (damageable != null && !hitPlayer)
            {
                damageable.TakeDamage((config as AreaEffectConfig).DamageToEachTarget);
            }
        }
    }

		
    }

}