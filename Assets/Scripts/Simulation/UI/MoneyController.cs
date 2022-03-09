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
		string spent = Format(Simulation.spent);
		string target = Format(Simulation.spendTarget);
		int percent = Mathf.FloorToInt((Simulation.spent / Simulation.spendTarget) * 100);

		text.SetText($"{spent}/£{target} ({percent}%)");
	}
}
