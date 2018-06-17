using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Weapons; //TODO Consider refactoring

namespace RPG.Characters
{
	public class Enemy : MonoBehaviour, IDamageable {

		[SerializeField] int maxHealthPoints = 100;
		[SerializeField] float currentHealthPoints;
		[SerializeField] float chaseRadius = 10f;

		//Attack Stats
		[SerializeField] float damagePerShot = 9f;
		[SerializeField] float attackRadius = 4f;
		[SerializeField] float firingPeriodInBetweenShots = 0.5f;
		[SerializeField] float firingPeriodVariation = 0.1f;
		[SerializeField] Projectile projectileToUse;
		[SerializeField] GameObject projectileSocket;
		[SerializeField] Vector3 aimOffset = new Vector3(0,1,0);

		
		Player player = null;
		AICharacterControl aiCharacterControl;
		Coroutine spawnProjectileCoroutine;

		bool isAttacking = false;

		public float healthAsPercentage{ get{ return currentHealthPoints / (float)maxHealthPoints; } }

		public void TakeDamage(float damage)
		{
			currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
			if (currentHealthPoints <= 0){
				Destroy(gameObject);
			}
		}

		void Start(){
			currentHealthPoints = maxHealthPoints;
			player = FindObjectOfType<Player>();
			aiCharacterControl = GetComponent<AICharacterControl>();
		}

		void Update(){
			if (player.healthAsPercentage <= Mathf.Epsilon){
				StopAllCoroutines();
				Destroy(this); //Stop enemy behavior
			}

			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			if (distanceToPlayer < attackRadius && !isAttacking){
				isAttacking = true;
				spawnProjectileCoroutine = StartCoroutine(FireProjectile()); //TODO slow this down
			}
			if (distanceToPlayer > attackRadius && spawnProjectileCoroutine != null){
				isAttacking = false;
				StopCoroutine(spawnProjectileCoroutine);
			}
			if (distanceToPlayer < chaseRadius){
				aiCharacterControl.SetTarget(player.transform);
			}else{
				aiCharacterControl.SetTarget(transform);
			}
		}

		IEnumerator FireProjectile(){
			while (true)
			{
				var randomisedDelay = firingPeriodInBetweenShots + Random.Range(-firingPeriodVariation, firingPeriodVariation);
				yield return new WaitForSeconds(randomisedDelay);
				Projectile projectileComponent = SpawnProjectile();
				Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
				projectileComponent.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileComponent.GetDefaultLaunchSpeed();
			}
		}

		private Projectile SpawnProjectile()
		{
			Projectile projectileComponent = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
			projectileComponent.SetDamage(damagePerShot);
			projectileComponent.SetShooter(gameObject);
			return projectileComponent;
		}

		void OnDrawGizmos(){
			//Draw the move sphere
			Gizmos.color = new Color(0, 0, 255f,.5f);
			Gizmos.DrawWireSphere(transform.position, chaseRadius);
			//Draw the attack sphere
			Gizmos.color = new Color(255f, 0, 0,.5f); //Color.red
			Gizmos.DrawWireSphere(transform.position, attackRadius);	
		
		}
	}
}
