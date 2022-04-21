using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextPurchaseController : MonoBehaviour {
	[SerializeField] private NFTMatchGrid dataScript;
	[SerializeField] private GameObject buyButton;

	private TextMeshProUGUI text;

	private int lastMatchesNeeded = -1;

	private void Awake() {
		text = GetComponent<TextMeshProUGUI>();
		UpdateIfNeeded();
	}

	private void FixedUpdate() {
		UpdateIfNeeded();
	}

	private void UpdateIfNeeded() {
		int matchesNeeded = Simulation.currentSave.NFTMatchSave.matchesUntilNFT;
		if (matchesNeeded != lastMatchesNeeded) {
			UpdateDisplay();
			lastMatchesNeeded = matchesNeeded;
		}
	}

	public void UpdateDisplay() {
		int matchesNeeded = Simulation.currentSave.NFTMatchSave.matchesUntilNFT;
		if (matchesNeeded > 0) {
			text.text = $"Next NFT Purchase Unlock:\n{matchesNeeded} more matched NFTs";
			gameObject.SetActive(true);
			buyButton.SetActive(false);
			Debug.Log("G");
		}
		else {
			gameObject.SetActive(false);
			Debug.Log("H");
			buyButton.SetActive(true);
		}
	}
}
