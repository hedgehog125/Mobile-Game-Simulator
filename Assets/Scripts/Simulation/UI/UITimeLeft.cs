using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimeLeft : MonoBehaviour {
    private TextMeshProUGUI text;

	private void Awake() {
		text = GetComponent<TextMeshProUGUI>();
		UpdateText();
	}

	private string ZeroPad(string textNum, int targetLength) {
		return new string('0', targetLength - textNum.Length) + textNum;
	}

	private void FixedUpdate() {
		UpdateText();
	}

	private void UpdateText() {
		Save save = Simulation.currentSave;

		if (
			Simulation.inGame && (! Simulation.revisitingGame)
			&& save.timeLeft != Simulation.difficulty.gameTimeLimit
		) {
			text.enabled = true;
		}
		else {
			text.enabled = false;
			return;
		}

		int ticksLeft = save.timeLeft;

		// An in game minute is a real life second so not dividing by as much
		int minutes = Mathf.FloorToInt(ticksLeft / 3000);
		ticksLeft -= minutes * 3000;
		int halfSeconds = Mathf.FloorToInt(ticksLeft / 25);
		int seconds = Mathf.FloorToInt(ticksLeft / 50);

		string minsText = ZeroPad(minutes.ToString(), 2);
		string secondsText = ZeroPad(seconds.ToString(), 2);

		bool showDot = (! Simulation.inGame) || (halfSeconds % 2 == 0);
		if (showDot) {
			text.SetText($"{minsText}:{secondsText}");
		}
		else {
			text.SetText($"{minsText} {secondsText}");
		}
	}
}
