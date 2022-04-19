using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NFTMatchMiscController : MonoBehaviour {
	[SerializeField] private string introScene;

	private void Awake() {
		if (! Simulation.currentSave.NFTMatchSave.opened) {
			Simulation.preventClose = true;
			SceneManager.LoadScene(introScene);
		}
	}
}
