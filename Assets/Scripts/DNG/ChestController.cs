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
	[SerializeField] private int pauseDelay;
	[SerializeField] private int finishDelay;

	private Animator anim;
	private Vector2 mousePos;

	private int spawnTick;
	private NFTController NFTOb;
	private bool animating;

	private enum States {
		WaitToOpen,
		Open,
		WaitToClose,
		Close
	}
	private States state = States.WaitToOpen;

	private void Awake() {
		anim = GetComponent<Animator>();
	}

	private void OnPoint(InputValue input) {
		mousePos = input.Get<Vector2>();
	}
	private void OnClick(InputValue input) {
		if (input.isPressed) {
			if (state == States.WaitToOpen) {
				state = States.Open;

				Ray ray = Camera.main.ScreenPointToRay(mousePos);
				if (Physics.Raycast(ray)) {
					keyObject.SetActive(true); // Play the animation
				}
			}
			else if (state == States.WaitToClose) {
				NFTOb.Fly(); // Next stage
				state = States.Close;
			}
		}
	}

	private void FixedUpdate() {
		if (animating) {
			if (state == States.Open) {
				if (spawnTick == spawnDelay) {
					NFTOb = Instantiate(NFTPrefab, NFTHolder).GetComponent<NFTController>();
					NFTOb.Randomize();
					NFTOb.Ready();
				}
				if (spawnTick == pauseDelay) {
					PauseAnimation();
					state = States.WaitToClose;
				}
			}
			else if (state == States.Close) {
				if (spawnTick == finishDelay) {
					ResetAnimation();
				}
			}
			spawnTick++;
		}
	}

	public void StartAnimation() {
		anim.SetBool("Start", true);
		anim.SetBool("Reset", false);

		spawnTick = 0;
		animating = true;
	}

	public void PauseAnimation() {
		anim.enabled = false;
		animating = false;
	}

	public void ResumeAnimation() {
		anim.enabled = true;
		animating = true;
	}

	public void ResetAnimation() {
		animating = false;
		anim.SetBool("Start", false);
		anim.SetBool("Reset", true);

		state = States.WaitToOpen;
	}
}
