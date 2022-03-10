using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SimulationController : MonoBehaviour { // Handles anything that needs to continually change over time in the Simulation global state
    [SerializeField] private bool isGame;
	[SerializeField] private string backScene = "PhoneMenu";

	private static bool hasGlobalController;
	private bool isTheGlobalController;

	private void OnClose(InputValue input) {
		if (input.isPressed) {
			SceneManager.LoadScene(backScene);
		}
	}

	private void Awake() { // Only called when it's first initialised, not when it's kept between scenes
		Simulation.inGame = isGame;
		if (! hasGlobalController) {
			DontDestroyOnLoad(gameObject);
			hasGlobalController = true;
			isTheGlobalController = true;
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
	}
}
