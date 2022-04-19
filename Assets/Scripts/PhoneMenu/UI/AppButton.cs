using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppButton : MonoBehaviour {
	[SerializeField] private string sceneName;
	[SerializeField] private int unlockID;

    public void OnClick() {
		SceneManager.LoadScene(sceneName);
	}

	private void Awake() {
		if (Simulation.currentSave.gamesUnlocked <= unlockID) {
			gameObject.SetActive(false);
		}
	}
}
