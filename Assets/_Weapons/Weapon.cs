using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
	[CreateAssetMenu(menuName = ("RPG/weapon"))]
	public class Weapon : ScriptableObject {

		public Transform gripTransform;

		[SerializeField] GameObject weaponPrefab;
		[SerializeField] AnimationClip attackAnimation;

		public GameObject GetWeaponPrefab(){
			return weaponPrefab;
		}

		public AnimationClip getAnimClip()
        {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        private void RemoveAnimationEvents()
        {
            attackAnimation.events = new AnimationEvent[0];
        }
    }
}