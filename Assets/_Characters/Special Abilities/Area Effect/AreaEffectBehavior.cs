﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {

public class AreaEffectBehavior : AbilityBehavior {

	AreaEffectConfig config;

	public AreaEffectConfig Config{ set { this.config = value;}}

	public override void Use(AbilityUseParams parameters)
        {
            DealRadialDamage();
			PlayParticleEffect();
        }

        private void DealRadialDamage()
        {
            RaycastHit[] hits;
            // Static sphere cast for targets
            hits = Physics.SphereCastAll(transform.position, config.Radius, Vector3.up, config.Radius);
            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                bool hitPlayer = hit.collider.GetComponent<Player>();
                if (damageable != null && !hitPlayer)
                {
                    damageable.TakeDamage(config.DamageToEachTarget);
                }
            }
        }

		private void PlayParticleEffect()
        {
            var particlePrefab = config.ParticlePrefab;
			var prefab = Instantiate(particlePrefab, transform.position, particlePrefab.transform.rotation);
			ParticleSystem particleSystem = prefab.GetComponent<ParticleSystem>();
			particleSystem.Play();
			Destroy(prefab, particleSystem.main.duration);
        }
    }

}