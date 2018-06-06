using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Projectile : MonoBehaviour {
	
	public float projectileSpeed;
	float damageCaused;

	public void SetDamage(float damage){
		damageCaused = damage;
	}

	void OnCollisionEnter (Collision collision){
		if (collision.gameObject.tag == "Player"){
			Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
			if (damageableComponent){
				(damageableComponent as IDamageable).TakeDamage(damageCaused);
			}
		}
		Destroy(gameObject, 0.01f);
		
	}
}
