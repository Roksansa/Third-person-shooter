using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class StartupController : MonoBehaviour {
	[SerializeField] private Slider progressBar;
	public static Managers Managers;
	void Awake() {
		StartCoroutine(StartController());
		
	}

	private IEnumerator StartController() {
		Managers = GameObject.Find("ManagerController").GetComponent<Managers>();
		while (Managers == null) {
			yield return null;
		}
		Managers = GameObject.Find("ManagerController").GetComponent<Managers>();
		Managers.UpdateLoadManager += OnManagersProgress;
		Managers.FinishesLoad += OnManagersStarted;
		Debug.Log("Listeners add");
	}

	void OnDestroy() {
		Managers.UpdateLoadManager -= OnManagersProgress;
		Managers.FinishesLoad -= OnManagersStarted;
	}
	private void OnManagersProgress(int numReady, int numModules) {
		float progress = (float)numReady / numModules;
		progressBar.value = progress; 
	}
	private void OnManagersStarted() {
		Managers.Mission.GoToNext();
	}
}
