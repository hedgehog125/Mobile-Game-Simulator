using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIVarToText : MonoBehaviour {
	[SerializeField] private string prefix;
	[SerializeField] private Options valueName;
	private enum Options {
		KnowledgePoints,
		Score
	};

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
		int value = 0;
		if (valueName == Options.KnowledgePoints) {
			value = Simulation.currentSave.knowledgePoints;
		}

		string formatted = Format(value);
		text.SetText($"{prefix}{formatted}");
	}
}
