using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestController : MonoBehaviour {
	private Animator anim;

	private Vector2 mousePos;

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
				anim.enabled = true; // Play the animation
			}
		}
	}
}
