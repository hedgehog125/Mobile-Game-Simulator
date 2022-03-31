using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyController : MonoBehaviour {
	private TextMeshProUGUI text;

	private void Awake() {
		text = GetComponent<TextMeshProUGUI>();
		UpdateText();
	}

	private string Format(int num) {
		return string.Format("{0:n0}", num);
	}

	private void FixedUpdate() {
		UpdateText();
	}

	private void UpdateText() {
		string spent = Format(Simulation.currentSave.spent);
		string target = Format(Simulation.currentSave.spendTarget);
		int percent = Mathf.FloorToInt((Simulation.currentSave.spent / Simulation.currentSave.spendTarget) * 100);

		text.SetText($"{spent}/£{target} ({percent}%)");
	}
}
