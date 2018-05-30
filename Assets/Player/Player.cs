using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable {

	[SerializeField] int enemyLayer = 9;
	[SerializeField] int maxHealthPoints = 100;
	[SerializeField] float currentHealthPoints;
	[SerializeField] int damagePerHit = 10;
	[SerializeField] float minTimeBetweenHits = .5f;
	[SerializeField] float maxHitRange = 2f;

	GameObject currentTarget;
	CameraRaycaster cameraRaycaster;
	float lastHitTime = 0f;

	public float healthAsPercentage{ get { return currentHealthPoints / (float)maxHealthPoints; } }

	public void Start(){
		currentHealthPoints = maxHealthPoints;
		cameraRaycaster = FindObjectOfType<CameraRaycaster>();
		cameraRaycaster.notifyMouseClickObservers += OnMouseClick; //registering

	}

	void OnMouseClick(RaycastHit raycastHit, int layerHit){
		if (layerHit == enemyLayer){
			var enemy = raycastHit.collider.gameObject;
			currentTarget = enemy;
			// Check enemy is in range
			if ((enemy.transform.position - transform.position).magnitude > maxHitRange){ return; }
			var enemyComponent = enemy.GetComponent<Enemy>();
			if (Time.time - lastHitTime > minTimeBetweenHits){
				enemyComponent.TakeDamage(damagePerHit);
				lastHitTime = Time.time;
			}
			
		}  
	}

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
		if (currentHealthPoints <= 0){
			//Destroy(gameObject);
			print("player dead");
		}
	}
}
