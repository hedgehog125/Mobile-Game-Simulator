using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayController : MonoBehaviour {
    private TextMeshProUGUI text;

	private void Awake() {
		text = GetComponent<TextMeshProUGUI>();
		UpdateText();
	}

	private void FixedUpdate() {
		UpdateText();
	}

	private void UpdateText() {
		text.SetText($"Day {Simulation.day + 1}");
	}
}
