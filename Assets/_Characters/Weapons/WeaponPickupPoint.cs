using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{
	[ExecuteInEditMode]
	public class WeaponPickupPoint : MonoBehaviour {

		[SerializeField] Weapon weaponConfig;
		[SerializeField] AudioClip pickupSFX;

		AudioSource audioSource = null;

		void Start(){
			audioSource = GetComponent<AudioSource>();
		}

		void Update(){
			if (!Application.isPlaying){
				DestroyChildren();
				InstantiateWeapon();
			}
			
		}

        private void DestroyChildren()
        {
			foreach (Transform child in transform){
				DestroyImmediate(child.gameObject);
			}
        }

        private void InstantiateWeapon()
        {
            var weapon = weaponConfig.GetWeaponPrefab();
			weapon.transform.position = Vector3.zero;
			Instantiate(weapon, transform);
        }

		void OnTriggerEnter(Collider collider){
			Player player = collider.gameObject.GetComponent<Player>();
			if (player){
				audioSource.PlayOneShot(pickupSFX);
				player.EquipWeapon(weaponConfig);
			}
		}
    }
}
