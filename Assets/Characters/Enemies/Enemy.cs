using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

	[SerializeField] int maxHealthPoints = 100;
	[SerializeField] float currentHealthPoints;
	[SerializeField] float chaseRadius = 10f;

	//Attack Stats
	[SerializeField] float damagePerShot = 9f;
	[SerializeField] float attackRadius = 4f;
	[SerializeField] float secondsBetweenShot = 0.5f;
	[SerializeField] Projectile projectileToUse;
	[SerializeField] GameObject projectileSocket;
	[SerializeField] Vector3 aimOffset = new Vector3(0,1,0);

	
	GameObject player = null;
	ThirdPersonCharacter thirdPersonCharacter = null;
	AICharacterControl aiCharacterControl;
	Coroutine spawnProjectileCoroutine;

	bool isAttacking = false;

	public float healthAsPercentage{ get{ return currentHealthPoints / (float)maxHealthPoints; } }

	public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
		if (currentHealthPoints <= 0){
			Destroy(gameObject);
			print("enemy dead");
		}
	}

    void Start(){
		currentHealthPoints = maxHealthPoints;
		player = GameObject.FindGameObjectWithTag("Player");
		thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
		aiCharacterControl = GetComponent<AICharacterControl>();
	}

	void Update(){
		float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
		if (distanceToPlayer < attackRadius && !isAttacking){
			isAttacking = true;
			spawnProjectileCoroutine = StartCoroutine(SpawnProjectile()); //TODO slow this down
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

	IEnumerator SpawnProjectile(){
		while (true){
			yield return new WaitForSeconds(secondsBetweenShot);
			Projectile newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
			newProjectile.SetDamage(damagePerShot);
			Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
			newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * newProjectile.projectileSpeed;
		}
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
