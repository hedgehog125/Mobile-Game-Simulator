using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UINFTScore : MonoBehaviour {
	[SerializeField] private int scoreLength;

	private TextMeshProUGUI tex;

	private void Awake() {
		tex = GetComponent<TextMeshProUGUI>();

		UpdateText();
	}
	private void FixedUpdate() {
		UpdateText();
	}

	private void UpdateText() {
		string scoreText = Simulation.currentSave.NFTMatchSave.score.ToString();
		scoreText = scoreText.PadLeft(scoreLength, '0');

		tex.text = $"Score: {scoreText}";
	}
}
