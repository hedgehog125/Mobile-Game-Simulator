using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FactPageController : MonoBehaviour { // I don't feel like trying to set up some sort of inheritance so I'm just going to recreate a simpler version of CutsceneTextController
	[Header("Objects and references")]
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI bodyText;
	[SerializeField] private GameObject buttonPrompt;

	[SerializeField] private int autoShowDelay = 75;

	private int autoShowTick;
	private int deactivateTick;

	private void OnAdvance(InputValue input) {
		if (input.isPressed) {
			if (deactivateTick == 0) {
				deactivateTick = 1;
			}
		}
	}

	private void FixedUpdate() {
		if (deactivateTick == 0) {
			if (autoShowTick == autoShowDelay) {
				buttonPrompt.SetActive(true);
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
	}

	public void Display(string title, string body) {
		titleText.text = title;
		bodyText.text = body;
		autoShowTick = 0;
		deactivateTick = 0;

		gameObject.SetActive(true);
		buttonPrompt.SetActive(false);
	}
}
