using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CutsceneTextController : MonoBehaviour {
	[Header("Objects and references")]
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject textOb;
	[SerializeField] private GameObject buttonPrompt;

	[Header("")]
	[SerializeField] private List<string> displayText;
	[SerializeField] private int autoShowDelay;
	[SerializeField] private string nextScene;

	private int autoShowTick;
	private int textID;

	private void OnAdvance(InputValue input) {
		if (input.isPressed) {
			if (buttonPrompt.activeSelf) {
				textID++;
				if (textID == displayText.Count) {
					gameObject.SetActive(false);
					SceneManager.LoadScene(nextScene);
				}
				else {
					text.text = displayText[textID];
					autoShowTick = 0;
					buttonPrompt.SetActive(false);
				}
			}
			else {
				buttonPrompt.SetActive(true);
			}
		}
	}

	private void Awake() {
		text.text = displayText[0];
	}

	private void FixedUpdate() {
		if (autoShowTick == autoShowDelay) {
			buttonPrompt.SetActive(true);
		}
		else {
			autoShowTick++;
		}
	}
}
