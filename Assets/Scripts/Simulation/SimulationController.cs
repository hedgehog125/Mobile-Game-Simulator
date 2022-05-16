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
				Simulation.ChangeScene(backScene);
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
		Simulation.UpdateVars(knowledgePoints);

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
