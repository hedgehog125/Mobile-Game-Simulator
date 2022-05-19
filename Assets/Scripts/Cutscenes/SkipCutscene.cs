using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

public class SkipCutscene : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject visible;
	[SerializeField] private Slider holdEffectUI;
	[SerializeField] private ChangeSceneOnVideoEnd videoEndListener;
	[SerializeField] private VideoPlayer videoToSkip;

	[Header("")]
    [SerializeField] private int holdTime;

	private bool canSkip;
    private bool isDown;
    private int holdTick;
	private float holdTickFloat;

    private void OnAdvance(InputValue input) {
        isDown = input.isPressed;
	}

	private void Awake() {
		UpdateUI();
	}

	private void FixedUpdate() {
		canSkip = videoToSkip == null || videoToSkip.isPlaying;

		if (canSkip) {
			if (isDown) {
				if (holdTick == holdTime) {
					isDown = false;

					if (videoEndListener != null) videoEndListener.Finish();
					if (videoToSkip != null) videoToSkip.Pause();
				}
				else {
					holdTick++;
				}
			}
			else {
				holdTick = 0;
				holdTickFloat = 0;
			}
		}
	}

	private void Update() {
		if (canSkip) {
			if (isDown) {
				holdTickFloat += Time.deltaTime / (holdTime / 50f);
			}
			UpdateUI();
		}
	}

	private void UpdateUI() {
		visible.SetActive(isDown);
		holdEffectUI.value = holdTickFloat;
	}
}
