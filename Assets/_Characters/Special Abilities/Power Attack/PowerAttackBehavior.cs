using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{
	public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility {

		PowerAttackConfig config;
        Player player;

		public PowerAttackConfig Config{ set { this.config = value;}}

        public void Start(){
		    player = gameObject.GetComponent<Player>();
	    }

		public void Use(AbilityUseParams parameters)
        {
            DealExtraDamage(parameters);
			PlayParticleEffect();
        }

        private void DealExtraDamage(AbilityUseParams parameters)
        {
            float damage = config.ExtraDamage + parameters.baseDamage;
            parameters.target.TakeDamage(damage);
            player.GetComponent<Animator>().SetTrigger("Attack");
        }


        //TODO Move to parent
		private void PlayParticleEffect()
        {
			var prefab = Instantiate(config.ParticlePrefab, transform.position, Quaternion.identity);
			ParticleSystem particleSystem = prefab.GetComponent<ParticleSystem>();
			particleSystem.Play();
			Destroy(prefab, particleSystem.main.duration);
        }
    }
}