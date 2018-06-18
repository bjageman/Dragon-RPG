﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters
{
	[ExecuteInEditMode]
	public class Projectile : MonoBehaviour {
		
		[SerializeField] float projectileSpeed;
		[SerializeField] GameObject shooter; //So can inspect when paused

		const float DESTROY_DELAY = 0.01f;

		float damageCaused;

		public void SetShooter(GameObject shooter){
			this.shooter = shooter;
		}

		public void SetDamage(float damage){
			damageCaused = damage;
		}

		public float GetDefaultLaunchSpeed(){
			return projectileSpeed;
		}

		void OnCollisionEnter (Collision collision){
			var layerCollidedWith = collision.gameObject.layer;
			if (shooter && layerCollidedWith != shooter.layer)
			{
				DealDamage(collision);
			}
			Destroy(gameObject, DESTROY_DELAY);
			
		}

		private void DealDamage(Collision collision)
		{
			Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
			if (damageableComponent)
			{
				(damageableComponent as IDamageable).TakeDamage(damageCaused);
			}
		}
	}
}
