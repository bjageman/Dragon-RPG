using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{
public class SelfHealBehavior : AbilityBehavior {

	Player player = null;

	public void Start(){
		player = gameObject.GetComponent<Player>();
	}

	public override void Use(AbilityUseParams parameters)
        {
            player.Heal((config as SelfHealConfig).HealAmount);
            PlayAbilitySound();
            PlayParticleEffect();

    }

        

}
}