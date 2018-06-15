using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters {

[CreateAssetMenu(menuName = ("RPG/Special Ability/Area Effect"))]
public class AreaEffectConfig : SpecialAbility {

	[Header("Area Effect Specific")]
	[SerializeField] float radius = 10f;
	[SerializeField] float damageToEachTarget = 10f;

	public float Radius { get { return this.radius; }}
	public float DamageToEachTarget { get { return this.damageToEachTarget; }}

	public override void AddComponent(GameObject gameObjectToAttachTo){
		var behaviorComponent = gameObjectToAttachTo.AddComponent<AreaEffectBehavior>();
		behaviorComponent.Config = this;
		behavior = behaviorComponent;
	}
}

}
