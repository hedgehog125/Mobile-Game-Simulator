using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SimulationController : MonoBehaviour { // Handles anything that needs to continually change over time in the Simulation global state
	// In the main controller, these values will still be the initial values when switching scenes, the Simulation class has the current values
    [SerializeField] private bool isGame;
	[SerializeField] private int gameID = -1;
	[SerializeField] public string backScene = "PhoneMenu";
	[SerializeField] private GameObject textAndFactOb;
	[SerializeField] public List<int> knowledgePoints;

	private CutsceneTextController myTextbox;
	private FactPageController myFactBox;

	private static SimulationController globalController;
	private bool isTheGlobalController;
	private PlayerInput inputModule;

	private int gotAllTick;

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
			globalController.knowledgePoints = knowledgePoints;

			SyncToSimulation();
		}

		UpdatePlays();
	}

	private void FixedUpdate() {
		if (isTheGlobalController) {
			Tick();
		}
	}

	private void Tick() {
		UpdateSimulationVars();

		if (Simulation.inGame) {
			if (
				((! Simulation.menuPopupActive) || Simulation.stayOnLastActive)
				&& (! Simulation.revisitingGame)
			) {
				Simulation.IncreaseTime(1);

				if (Simulation.gotAllInGame) {
					if (gotAllTick == 60) {
						Simulation.GotAllPoints();
						gotAllTick = 0;
					}
					else {
						gotAllTick++;
					}
				}
				else {
					gotAllTick = 0;
				}
			}
		}
	}

	private void UpdateSimulationVars() {
		bool textBoxShowing = Simulation.textBox == null? false : Simulation.textBox.gameObject.activeSelf;
		bool factBoxShowing = Simulation.factBox == null? false : Simulation.factBox.gameObject.activeSelf;

		Simulation.menuPopupActive = textBoxShowing || factBoxShowing;
		Simulation.stayOnLastActive = textBoxShowing? Simulation.textBox.stayOnLast : false;
		Simulation.revisitingGame = gameID == 0 || gameID == -1? false : Simulation.currentSave.gamesUnlocked != gameID;

		bool gotAll = true;
		if (knowledgePoints.Count != 0) {
			foreach (int id in knowledgePoints) {
				if (! Simulation.currentSave.knowledgePointsGot[id]) {
					gotAll = false;
					break;
				}
			}
		}
		Simulation.gotAllInGame = gotAll;
	}

	public void SyncToSimulation() {
		Simulation.textBox = myTextbox;
		Simulation.factBox = myFactBox;

		Simulation.inGame = isGame;
		Simulation.gameID = gameID;
	}

	private void UpdatePlays() {
		bool firstGamePlay = false;

		Save save = Simulation.currentSave;
		if (Simulation.gameID == 1) {
			if (save.DNGSave.plays == 0) {
				firstGamePlay = true;
			}
			save.DNGSave.plays++;
		}
		else if (Simulation.gameID == 2) {
			if (save.NFTMatchSave.plays == 0) {
				firstGamePlay = true;
			}
			save.NFTMatchSave.plays++;
		}

		Simulation.firstGamePlay = firstGamePlay;
	}
}
