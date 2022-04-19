using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkipCutscene : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject visible;
	[SerializeField] private Slider holdEffectUI;
	[SerializeField] private ChangeSceneOnVideoEnd videoEndListener;

	[Header("")]
    [SerializeField] private int holdTime;

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
		if (isDown) {
			if (holdTick == holdTime) {
				gameObject.SetActive(false);
				if (videoEndListener != null) videoEndListener.Finish();
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

	private void Update() {
		if (isDown) {
			holdTickFloat += Time.deltaTime / (holdTime / 50f);
		}
		UpdateUI();
	}

	private void UpdateUI() {
		visible.SetActive(isDown);
		holdEffectUI.value = holdTickFloat;
	}
}
