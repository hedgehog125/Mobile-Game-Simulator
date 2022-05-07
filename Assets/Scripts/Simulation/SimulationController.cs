using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SimulationController : MonoBehaviour { // Handles anything that needs to continually change over time in the Simulation global state
	// In the main controller, these values will still be the initial values when switching scenes, the Simulation class has the current values
    [SerializeField] private bool isGame;
	[SerializeField] private string gameName;
	[SerializeField] public string backScene = "PhoneMenu";
	[SerializeField] private GameObject textAndFactOb;

	private CutsceneTextController myTextbox;
	private FactPageController myFactBox;

	private static SimulationController globalController;
	private bool isTheGlobalController;
	private PlayerInput inputModule;

	private void OnClose(InputValue input) {
		if ((! Simulation.preventClose) && backScene != "") {
			if (input.isPressed) {
				SceneManager.LoadScene(backScene);
			}
		}
	}

	private void Awake() { // Only called when it's first initialised, not when it's kept between scenes
		inputModule = GetComponent<PlayerInput>();
		myTextbox = textAndFactOb.transform.GetChild(0).GetComponent<CutsceneTextController>();
		myFactBox = textAndFactOb.transform.GetChild(1).GetComponent<FactPageController>();


		if (globalController == null) {
			DontDestroyOnLoad(gameObject);
			globalController = this;
			isTheGlobalController = true;
			inputModule.enabled = true;

			SyncToSimulation();
			Simulation.Init();
		}
		else {
			globalController.backScene = backScene;

			SyncToSimulation();
		}
	}

	private void FixedUpdate() {
		if (isTheGlobalController) {
			Tick();
		}
	}

	private void Tick() {
		if (Simulation.inGame) {
			Simulation.IncreaseTime(1);
		}

		bool textBoxShowing = Simulation.textBox == null? false : Simulation.textBox.gameObject.activeSelf;
		bool factBoxShowing = Simulation.factBox == null? false : Simulation.factBox.gameObject.activeSelf;

		Simulation.menuPopupActive = textBoxShowing || factBoxShowing;
	}

	public void SyncToSimulation() {
		Simulation.textBox = myTextbox;
		Simulation.factBox = myFactBox;

		Simulation.inGame = isGame;
		Simulation.gameName = gameName;
	}
}
