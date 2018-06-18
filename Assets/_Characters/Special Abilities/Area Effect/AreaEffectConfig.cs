using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Characters {

[CreateAssetMenu(menuName = ("RPG/Special Ability/Area Effect"))]
public class AreaEffectConfig : AbilityConfig {

	[Header("Area Effect Specific")]
	[SerializeField] float radius = 10f;
	[SerializeField] float damageToEachTarget = 10f;

	public float Radius { get { return this.radius; }}
	public float DamageToEachTarget { get { return this.damageToEachTarget; }}

	public override AbilityBehavior GetBehaviorComponent(GameObject objectToAttachTo){
			return objectToAttachTo.AddComponent<AreaEffectBehavior>();
		}
}

}
