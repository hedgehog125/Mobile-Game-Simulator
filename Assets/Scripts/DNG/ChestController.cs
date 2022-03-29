using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestController : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject keyObject;
	[SerializeField] private GameObject NFTPrefab;
	[SerializeField] private Transform NFTHolder;
	[SerializeField] private GameObject limitPopup;

	[Header("Delays")]
	[SerializeField] private int spawnDelay;
	[SerializeField] private int pauseDelay;
	[SerializeField] private int finishDelay;

	[Header("Misc")]
	[SerializeField] private int cost;
	[SerializeField] private int limit;

	private Animator anim;
	private bool clicking;
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
		clicking = input.isPressed;
	}

	private void FixedUpdate() {
		if (clicking) {
			if (! limitPopup.activeSelf) {
				if (state == States.WaitToOpen) {
					Ray ray = Camera.main.ScreenPointToRay(mousePos);
					if (Physics.Raycast(ray)) {
						Open();
					}
					else {
						clicking = false;
					}
				}
				else if (state == States.WaitToClose) {
					NFTOb.Fly(); // Next stage
					state = States.Close;
				}
			}
		}

		if (animating) {
			if (state == States.Open) {
				if (spawnTick == spawnDelay) {
					NFTOb = Instantiate(NFTPrefab, NFTHolder).GetComponent<NFTController>();
					Save.ownedNFTs.Add(NFTOb.Randomize());
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

	private void Open() {
		if (Simulation.dailyLimitProgress.DNG == limit) {
			limitPopup.SetActive(true);
		}
		else {
			state = States.Open;
			keyObject.SetActive(true); // Play the animation
			Simulation.Spend(cost);
			Simulation.dailyLimitProgress.DNG++;
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
