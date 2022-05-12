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
	[SerializeField] private GameObject buttonPrompt;

	[Header("")]
	[SerializeField] private List<string> displayText;
	[SerializeField] private int autoShowDelay = 75;
	[SerializeField] public string nextScene;
	[SerializeField] public bool stayOnLast;
	[SerializeField] private bool deactivateIfWatched = true;

	private int autoShowTick;
	private int textID;
	public bool onLast { get; private set; }

	private int deactivateTick;

	private void OnAdvance(InputValue input) {
		if (input.isPressed && deactivateTick == 0) {
			if (buttonPrompt.activeSelf) {
				textID++;
				if (textID == displayText.Count) {
					if (stayOnLast) {
						textID--;
					}
					else {
						if (nextScene == "") {
							deactivateTick = 1;
						}
						else {
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

	private void OnPrevious(InputValue input) {
		if (input.isPressed) {
			if (textID != 0) {
				textID--;

				text.text = displayText[textID];
				autoShowTick = 0;
				buttonPrompt.SetActive(false);
			}
		}
	}

	private void Awake() {
		if (deactivateIfWatched) {
			Save save = Simulation.currentSave;

			if (Simulation.gameID == 0) {
				if (save.watched.intro) {
					gameObject.SetActive(false);
				}
				save.watched.intro = true;
			}
			else if (Simulation.gameID != -1) {
				if (! Simulation.firstGamePlay) {
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
		if (deactivateTick == 0) {
			if (autoShowTick == autoShowDelay) {
				if ((! stayOnLast) || textID != displayText.Count - 1) {
					buttonPrompt.SetActive(true);
				}
			}
			else {
				autoShowTick++;
			}
		}
		else {
			if (deactivateTick == 2) {
				gameObject.SetActive(false);
			}
			else {
				deactivateTick++;
			}
		}

		onLast = textID == displayText.Count - 1;
	}

	public void Display(List<string> message) {
		displayText = message;

		textID = 0;
		autoShowTick = 0;
		nextScene = "";
		deactivateTick = 0;

		gameObject.SetActive(true);
		MultiAwake();
	}
}
