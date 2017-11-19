using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {

	void Start() {
	}

	public void Hurt(int damage) {
		Managers.Player.SetHealth(-damage); 
	}
}
