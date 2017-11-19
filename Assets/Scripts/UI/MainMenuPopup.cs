using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPopup : MonoBehaviour {

	public void ExitGame() {
		Debug.Log("Exit");
		Application.Quit();
	}
	
	public void SaveGame() {
		Managers.Data.SaveGameState();
	}

	public void LoadGame() {
		Managers.Data.LoadGameState();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
