using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrambleTextAnimation : StateMachineBehaviour {
    [SerializeField] private float scrambleDelay;
    [SerializeField] private float unscrambleEndTime;
    [SerializeField] private float unscrambleSpeedReduction;

    private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ,./\\[]<>`~§'";

    private string baseText;
    private TextMeshProUGUI text;
    private char[] charText;

    private float scrambleTick;
    private float totalTick;
    private float unscrambleStartTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        text = animator.gameObject.GetComponent<TextMeshProUGUI>();
        baseText = text.text;

        charText = new char[baseText.Length];
        for (int i = 0; i < baseText.Length; i++) {
            charText[i] = RandomChar();
		}
        UpdateText();

        totalTick = 0;
        unscrambleStartTime = unscrambleEndTime - (baseText.Length * scrambleDelay * unscrambleSpeedReduction);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        scrambleTick += Time.deltaTime;
        totalTick += Time.deltaTime;

        if (totalTick >= unscrambleStartTime) {
            if (scrambleTick >= scrambleDelay * unscrambleSpeedReduction) {
                scrambleTick -= scrambleDelay * unscrambleSpeedReduction;

                if (text.text == baseText) return;

                int index;
                do {
                    index = Random.Range(0, text.text.Length);
                } while (charText[index] == baseText[index]);

                charText[index] = baseText[index]; // Unscramble
                UpdateText();
            }
        }
        else {
            if (scrambleTick >= scrambleDelay) {
                scrambleTick -= scrambleDelay;

                charText[Random.Range(0, text.text.Length)] = RandomChar();
                UpdateText();
			}
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        text.text = baseText;
    }

    private char RandomChar() {
        return characters[Random.Range(0, characters.Length)];
	}

    private void UpdateText() {
        text.text = new string(charText);
	}
}