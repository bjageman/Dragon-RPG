using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{

	public abstract class SpecialAbilityConfig : ScriptableObject {

		[Header("Special Ability General")]
		[SerializeField] float energyCost = 10f;

		public float EnergyCost{
			get { return this.energyCost; }
		}

		abstract public ISpecialAbility AddComponent(GameObject gameObjectToAttachTo);

	}
}
