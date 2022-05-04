using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneTextController : MonoBehaviour {
	[Header("Objects and references")]
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject textOb;
	[SerializeField] private GameObject buttonPrompt;

	[Header("")]
	[SerializeField] private List<string> displayText;
	[SerializeField] private int autoShowDelay;
	[SerializeField] public string nextScene;
	[SerializeField] public bool stayOnLast;
	[SerializeField] private bool deactivateIfWatched = true;

	private int autoShowTick;
	private int textID;

	private void OnAdvance(InputValue input) {
		if (input.isPressed) {
			if (buttonPrompt.activeSelf) {
				textID++;
				if (textID == displayText.Count) {
					if (stayOnLast) {
						textID--;
					}
					else {
						gameObject.SetActive(false);
						if (nextScene != "") {
							SceneManager.LoadScene(nextScene);
						}
					}
				}
				else {
					text.text = displayText[textID];
					autoShowTick = 0;
					buttonPrompt.SetActive(false);
				}
			}
			else {
				if ((! stayOnLast) || textID != displayText.Count - 1) {
					buttonPrompt.SetActive(true);
				}
			}
		}
	}

	private void Awake() {
		if (deactivateIfWatched) {
			Save save = Simulation.currentSave;

			if (Simulation.gameName == "DNG") {
				if (save.DNGSave.plays != 0) {
					gameObject.SetActive(false);
				}
			}
			else if (Simulation.gameName == "NFTMatch") {
				if (save.NFTMatchSave.plays != 0) {
					gameObject.SetActive(false);
				}
			}
		}

		MultiAwake();
	}

	private void MultiAwake() {
		text.text = displayText[0];
	}

	private void FixedUpdate() {
		if (autoShowTick == autoShowDelay) {
			if ((! stayOnLast) || textID != displayText.Count - 1) {
				buttonPrompt.SetActive(true);
			}
		}
		else {
			autoShowTick++;
		}
	}

	public void Display(List<string> message) {
		displayText = message;

		textID = 0;
		autoShowTick = 0;
		nextScene = "";

		gameObject.SetActive(true);
		MultiAwake();
	}
}
