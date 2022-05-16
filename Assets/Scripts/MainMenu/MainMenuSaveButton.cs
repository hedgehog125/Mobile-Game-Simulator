using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSaveButton : MonoBehaviour {
	[SerializeField] private Types type;

	private enum Types {
		LoadSave,
		NewSave,
		ChangeSave,
		BackToMain
	}
	private Button but;

	public void OnClick() {
		if (type == Types.LoadSave) {

		}
		else if (type == Types.NewSave) {
			Simulation.currentSave = Simulation.NewSave();
			Simulation.StartPlaying();
		}
		else if (type == Types.ChangeSave) {

		}
		else {
			Simulation.BackToMainMenu();
		}
	}


	private void Awake() {
		but = GetComponent<Button>();

		if (type == Types.LoadSave) {
			Simulation.Init(false);
			but.interactable = false;
		}
		else if (type == Types.ChangeSave) {
			but.interactable = false;
		}
	}

	private void FixedUpdate() {
		if (type == Types.LoadSave) {
			but.interactable = Simulation.saves.Count != 0;
		}
		else if (type == Types.ChangeSave) {
			but.interactable = Simulation.saves.Count >= 2;
		}
	}
}
