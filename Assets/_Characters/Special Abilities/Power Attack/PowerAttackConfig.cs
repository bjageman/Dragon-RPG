using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
[CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
public class PowerAttackConfig : SpecialAbilityConfig {

	[Header("Power Attack Specific")]
	[SerializeField] float attackDamage = 10f;

	public float AttackDamage{
		get { return this.attackDamage; }
	}

	public override ISpecialAbility AddComponent(GameObject gameObjectToAttachTo){
		var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehavior>();
		behaviourComponent.SetConfig(this);
		return behaviourComponent;
	}
}
}