using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NFTMatchMiscController : MonoBehaviour {
	[SerializeField] private string introScene;

	private void Awake() {
		Save.NFTMatchSaveClass save = Simulation.currentSave.NFTMatchSave;

		save.plays++;
		if (save.plays == 0) { // Starts at -1
			Simulation.preventClose = true;
			SceneManager.LoadScene(introScene);
		}
	}
}
