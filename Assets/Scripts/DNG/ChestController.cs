using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestController : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject keyObject;
	[SerializeField] private GameObject NFTPrefab;
	[SerializeField] private Transform NFTHolder;

	[Header("Delays")]
	[SerializeField] private int spawnDelay;

	private Animator anim;
	private Vector2 mousePos;

	private int spawnTick;
	private bool spawnedNFT;

	private void Awake() {
		anim = GetComponent<Animator>();
	}

	private void OnPoint(InputValue input) {
		mousePos = input.Get<Vector2>();
	}
	private void OnClick(InputValue input) {
		if (input.isPressed) {
			Ray ray = Camera.main.ScreenPointToRay(mousePos);
			if (Physics.Raycast(ray)) {
				keyObject.SetActive(true); // Play the animation
			}
		}
	}

	private void FixedUpdate() {
		if (! spawnedNFT) {
			if (anim.enabled) {
				if (spawnTick == spawnDelay) {
					NFTController NFTOb = Instantiate(NFTPrefab, NFTHolder).GetComponent<NFTController>();
					NFTOb.Randomize();
					NFTOb.animate = true;
					NFTOb.Ready();

					spawnedNFT = true;
				}
				else {
					spawnTick++;
				}
			}
		}
	}
}
