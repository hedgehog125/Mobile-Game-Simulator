using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour {
    private TextMeshProUGUI text;

	private void Awake() {
		text = GetComponent<TextMeshProUGUI>();
	}

	private string ZeroPad(string textNum, int targetLength) {
		return new string('0', targetLength - textNum.Length) + textNum;
	}

	private void FixedUpdate() {
		int totalTicks = Simulation.time;

		// An in game minute is a real life second so not dividing by as much
		int hours = Mathf.FloorToInt(totalTicks / 3000);
		totalTicks -= hours * 3000;
		int halfMinutes = Mathf.FloorToInt(totalTicks / 25);
		int mins = Mathf.FloorToInt(totalTicks / 50);

		string hoursText = ZeroPad(hours.ToString(), 2);
		string minsText = ZeroPad(mins.ToString(), 2);

		bool showDot = (! Simulation.inGame) || (halfMinutes % 2 == 0);
		if (showDot) {
			text.SetText($"{hoursText}:{minsText}");
		}
		else {
			text.SetText($"{hoursText} {minsText}");
		}
	}
}
