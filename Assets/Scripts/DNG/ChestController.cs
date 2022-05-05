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
	[SerializeField] private ParticleSystem particles;
	[SerializeField] private GameObject priceText;
	[SerializeField] private CutsceneTextController textBox;

	[Header("SFX")]
	[SerializeField] private AudioSource limitMusic;
	[SerializeField] private AudioSource closeSound;

	[Header("Delays")]
	[SerializeField] private int spawnDelay;
	[SerializeField] private int pauseDelay;
	[SerializeField] private int finishDelay;

	[Header("Misc")]
	[SerializeField] private int cost;
	[SerializeField] private int limit;
	[SerializeField] private List<string> firstOpenMessage;

	private Animator anim;
	private bool clicking;
	private Vector2 mousePos;

	private int spawnTick;
	private NFTController NFTOb;
	private bool animating;
	private bool firstOpenMessageShown;

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
			bool canOpen = ! (limitPopup.activeSelf || textBox.gameObject.activeSelf);
			if (canOpen) {
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
			else {
				clicking = false;
			}
		}

		if (animating) {
			if (state == States.Open) {
				if (spawnTick == spawnDelay) {
					NFTOb = Instantiate(NFTPrefab, NFTHolder).GetComponent<NFTController>();
					Simulation.currentSave.DNGSave.ownedNFTs.Add(NFTOb.Randomize());
					NFTOb.Ready();
				}
				if (spawnTick == pauseDelay) {
					PauseAnimation();
					state = States.WaitToClose;
				}
			}
			else if (state == States.Close) {
				if (spawnTick == finishDelay) {
					Finish();
				}
			}
			spawnTick++;
		}
	}

	private void Open() {
		Save.DNGSaveClass save = Simulation.currentSave.DNGSave;

		if (save.dailyLimitProgress == limit) {
			if (! limitPopup.activeSelf) {
				limitPopup.SetActive(true);
				limitMusic.Play();
			}
		}
		else {
			state = States.Open;
			keyObject.SetActive(true); // Play the animation
			Simulation.Spend(cost);
			save.dailyLimitProgress++;

			particles.Play();
			priceText.SetActive(false);
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

		particles.Stop();
	}

	public void ResumeAnimation() {
		anim.enabled = true;
		animating = true;

		closeSound.Play();
	}

	public void ResetAnimation() {
		animating = false;
		anim.SetBool("Start", false);
		anim.SetBool("Reset", true);

		state = States.WaitToOpen;
	}

	public void Finish() {
		ResetAnimation();

		priceText.SetActive(true);
		Debug.Log(firstOpenMessageShown);
		if (Simulation.currentSave.DNGSave.plays == 1 && (! firstOpenMessageShown)) {
			textBox.stayOnLast = true;
			textBox.Display(firstOpenMessage);

			firstOpenMessageShown = true;
		}
	}
}
