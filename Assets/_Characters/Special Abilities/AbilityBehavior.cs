using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{
	public abstract class AbilityBehavior : MonoBehaviour {

		protected AbilityConfig config;

		const float PARTICLE_CLEAN_UP_DELAY = 1f;

		public AbilityConfig Config{ set { this.config = value;}}

		public abstract void Use(AbilityUseParams parameters);
		
		protected void PlayParticleEffect()
        {
            var particlePrefab = config.ParticlePrefab;
			if (particlePrefab){
				var particleObject = Instantiate(
					particlePrefab, 
					transform.position, 
					particlePrefab.transform.rotation
				);
				particleObject.transform.parent = transform;
				particleObject.GetComponent<ParticleSystem>().Play();
				StartCoroutine(DestroyParticleWhenFinished(particleObject));
			}else{
				Debug.LogWarning("Particle prefab is missing");
			}
        }

		IEnumerator DestroyParticleWhenFinished(GameObject particlePrefab){
			while (particlePrefab.GetComponent<ParticleSystem>().isPlaying){
				yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
			}
			Destroy(particlePrefab);
			yield return new WaitForEndOfFrame();
		}

		protected void PlayAbilitySound()
        {
			var abilitySound = config.GetRandomSound();//TODO Change to random clip
            var audioSource = GetComponent<AudioSource>();
            if (abilitySound){
				//audioSource.clip = abilitySound;
            	//audioSource.Play();
				audioSource.PlayOneShot(abilitySound);
			}else{
				Debug.LogWarning("Audio clip is missing");
			}
			
        }

	}
}
