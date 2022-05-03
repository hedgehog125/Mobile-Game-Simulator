using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SimulationController : MonoBehaviour { // Handles anything that needs to continually change over time in the Simulation global state
    [SerializeField] private bool isGame;
	[SerializeField] public string backScene = "PhoneMenu";
	[SerializeField] private CutsceneTextController textBox;

	private static SimulationController globalController;
	private bool isTheGlobalController;
	private PlayerInput inputModule;

	private void OnClose(InputValue input) {
		if (isTheGlobalController && (! Simulation.preventClose) && backScene != "") {
			if (input.isPressed) {
				SceneManager.LoadScene(backScene);
			}
		}
	}

	private void Awake() { // Only called when it's first initialised, not when it's kept between scenes
		inputModule = GetComponent<PlayerInput>();

		Simulation.inGame = isGame;
		if (globalController == null) {
			DontDestroyOnLoad(gameObject);
			globalController = this;
			isTheGlobalController = true;
			inputModule.enabled = true;

			Simulation.Init();
		}
		else { // Set the variables of the global controller to the values provided by this new controller
			globalController.backScene = backScene;
			globalController.textBox = textBox;
		}

		SyncToSimulation();
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
	}

	public void SyncToSimulation() {
		Simulation.textBox = globalController.textBox;
	}
}
