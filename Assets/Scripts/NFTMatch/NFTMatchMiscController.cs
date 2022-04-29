using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NFTMatchMiscController : MonoBehaviour {

	private void Awake() {
		Save.NFTMatchSaveClass save = Simulation.currentSave.NFTMatchSave;

		save.plays++;
	}
}
