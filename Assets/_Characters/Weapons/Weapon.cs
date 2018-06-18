using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
	[CreateAssetMenu(menuName = ("RPG/weapon"))]
	public class Weapon : ScriptableObject {

		public Transform gripTransform;

		[SerializeField] GameObject weaponPrefab;
		[SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;

        public float AdditionalDamage { get { return this.additionalDamage; }}

        public float GetMinTimeBetweenHits()
        {
            // TODO consdier whether we take animation time into account
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }
        
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