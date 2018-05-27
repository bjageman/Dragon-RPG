using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour {

	[SerializeField] int maxHealthPoints = 100;
	[SerializeField] float currentHealthPoints = 100f;
	[SerializeField] float attackRadius = 4f;

	GameObject player = null;
	ThirdPersonCharacter thirdPersonCharacter = null;
	AICharacterControl aiCharacterControl;

	public float healthAsPercentage{
		get{
			return currentHealthPoints / (float)maxHealthPoints;
		}
	}

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
		aiCharacterControl = GetComponent<AICharacterControl>();
	}

	void Update(){
		float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
		if (distanceToPlayer < attackRadius){
			aiCharacterControl.SetTarget(player.transform);
		}else{
			aiCharacterControl.SetTarget(transform);
		}
	}
}
