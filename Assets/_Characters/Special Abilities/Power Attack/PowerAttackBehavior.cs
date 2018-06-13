using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters{
	public class PowerAttackBehavior : MonoBehaviour, ISpecialAbility {

		PowerAttackConfig config;

		public void SetConfig(PowerAttackConfig config){
			this.config = config;
		}

		void Start(){
		}

		public void Use(){

		}
	}
}