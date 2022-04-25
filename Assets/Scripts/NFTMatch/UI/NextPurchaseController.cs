using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPurchaseController : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private NFTMatchGrid dataScript;
	[SerializeField] private GameObject buyButton;

	[Header("")]
	[SerializeField] private float changeSpeed;

	private Slider slider;

	private void Awake() {
		slider = GetComponent<Slider>();
		UpdateDisplay();
	}

	private void Update() {
		UpdateDisplay();
	}

	public void UpdateDisplay() {
		int untilPurchase = Simulation.currentSave.NFTMatchSave.matchesUntilNFT;
		float maxFrameChange = changeSpeed * Time.deltaTime;
		float target = 1 - (untilPurchase / (float)dataScript.neededMatchesPerNFT);

		if (Mathf.Abs(target - slider.value) < maxFrameChange) {
			slider.value = target;
		}
		else {
			slider.value += maxFrameChange * (target > slider.value? 1 : -1);

			if (untilPurchase == 0) {

			}
		}
	}
}
