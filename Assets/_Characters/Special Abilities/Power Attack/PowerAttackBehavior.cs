using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{
	public class PowerAttackBehavior : AbilityBehavior  {

        Player player;

        public void Start(){
		    player = gameObject.GetComponent<Player>();
	    }

		public override void Use(AbilityUseParams parameters)
        {
            DealExtraDamage(parameters);
            PlayAbilitySound();
			PlayParticleEffect();
        }

        private void DealExtraDamage(AbilityUseParams parameters)
        {
            float damage = (config as PowerAttackConfig).ExtraDamage + parameters.baseDamage;
            parameters.target.TakeDamage(damage);
            player.GetComponent<Animator>().SetTrigger("Attack");
        }
    }
}