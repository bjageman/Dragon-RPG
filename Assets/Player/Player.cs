using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField] int maxHealthPoints = 100;
	[SerializeField] float currentHealthPoints = 100f;

	public float healthAsPercentage{
		get{
			return currentHealthPoints / (float)maxHealthPoints;
		}
	}
}
