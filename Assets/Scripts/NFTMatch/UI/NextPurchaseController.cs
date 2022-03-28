using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextPurchaseController : MonoBehaviour {
	[SerializeField] private NFTMatchGrid dataScript;
	[SerializeField] private GameObject buyButton;

	private TextMeshProUGUI text;

	private int lastTurns = 0;

	private void Awake() {
		text = GetComponent<TextMeshProUGUI>();
	}

	private void FixedUpdate() {
		int turns = dataScript.turnsUntilNFT;
		if (turns != lastTurns) {
			if (turns > 0) {
				text.text = $"Next NFT Purchase Unlock:\n{turns} more moves";
				gameObject.SetActive(true);
				buyButton.SetActive(false);
			}
			else {
				gameObject.SetActive(false);
				buyButton.SetActive(true);
			}
			lastTurns = dataScript.turnsUntilNFT;
		}
	}
}
