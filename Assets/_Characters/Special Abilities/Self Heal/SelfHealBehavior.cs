using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{
public class SelfHealBehavior : AbilityBehavior {

	SelfHealConfig config = null;
	Player player = null;
	AudioSource audioSource = null;

	public void Start(){
		player = gameObject.GetComponent<Player>();
	}

	public SelfHealConfig Config { set { this.config = value;} } 

	public override void Use(AbilityUseParams parameters)
        {
            player.Heal(config.HealAmount);
            PlayAudioClip();
            PlayParticleEffect();

    }

        private void PlayAudioClip()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = config.AudioClip;
            audioSource.Play();
        }

        private void PlayParticleEffect()
        {
			var prefab = Instantiate(config.ParticlePrefab, transform.position, Quaternion.identity);
			prefab.transform.parent = transform;
			ParticleSystem particleSystem = prefab.GetComponent<ParticleSystem>();
			particleSystem.Play();
            //TODO Particles killed too soon
			Destroy(prefab, 10);
        }
}
}