using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DayController : MonoBehaviour {
    private TextMeshProUGUI text;

	private void Awake() {
		text = GetComponent<TextMeshProUGUI>();
	}

	private void FixedUpdate() {
		text.SetText($"Day {Simulation.day + 1}");
	}
}
