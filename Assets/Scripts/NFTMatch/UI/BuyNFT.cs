using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BuyNFT : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject NFTPrefab;
	[SerializeField] private Transform NFTHolder;
	[SerializeField] private NFTMatchGrid matchScript;

	[Header("Cameras")]
	[SerializeField] private GameObject mainCam;
	[SerializeField] private GameObject NFTCam;

	[Header("Buttons")]
	[SerializeField] private GameObject buyButton;
	[SerializeField] private GameObject cancelButton;

	private Button but;
	private NFTController NFTOb;

	private Save.OwnedNFT pendingData;
	private bool waiting;
	private bool shownButtons;

	private void Awake() {
		but = GetComponent<Button>();
	}

	public void OnClick() {
		NFTOb = Instantiate(NFTPrefab, NFTHolder).GetComponent<NFTController>();
		NFTOb.resumeAnimation = false;

		pendingData = NFTOb.Randomize();
		NFTOb.Ready();

		mainCam.SetActive(false);
		NFTCam.SetActive(true);
		but.interactable = false;

		matchScript.inputPaused = true;
		waiting = true;
	}

	public void OnBuy() {
		Simulation.Spend(NFTOb.value);
		Save.ownedNFTs.Add(pendingData);

		OnEither();
	}
	public void OnDecline() {
		OnEither();
	}

	public void OnEither() {
		NFTOb.Fly();

		buyButton.SetActive(false);
		cancelButton.SetActive(false);
		pendingData = null;
	}

	private void FixedUpdate() {
		if (waiting) {
			if (NFTOb == null) {
				Finish();
				return;
			}
			else {
				if (! shownButtons) {
					if (NFTOb.canFly) {
						ShowButtons();
					}
				}
			}
		}
	}

	private void ShowButtons() {
		buyButton.SetActive(true);
		cancelButton.SetActive(true);

		shownButtons = true;
	}
	private void Finish() {
		shownButtons = false;
		waiting = false;

		mainCam.SetActive(true);
		NFTCam.SetActive(false);

		but.interactable = true;
		matchScript.inputPaused = false;
	}
}
