using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMoneySpent : MonoBehaviour {
	[SerializeField] private int updateTextDelay;
	[SerializeField] private int showTime;
	[SerializeField] private int totalLength;

    private TextMeshProUGUI tex;
	private Animator anim;

	private Save save;
	private int lastMoney;
	private int updateTextTick;
	private int showTick;

	private void Awake() {
		tex = GetComponent<TextMeshProUGUI>();
		anim = GetComponent<Animator>();

		save = Simulation.currentSave;
		lastMoney = save.spent;
	}

	private void FixedUpdate() {
		if (showTick == 0) {
			if (updateTextTick == 0) {
				if (lastMoney != save.spent) {
					UpdateText(lastMoney);
					anim.SetBool("Show", true);
					updateTextTick = 1;

					lastMoney = save.spent;
				}
			}
			else {
				if (updateTextTick == updateTextDelay) {
					UpdateText(save.spent);

					updateTextTick = 0;
					showTick = 1;
				}
				else {
					updateTextTick++;
				}
			}
		}
		else {
			if (showTick == showTime) {
				anim.SetBool("Show", false);

				showTick = 0;
			}
			else {
				showTick++;
			}
		}
	}

	private void UpdateText(int spent) {
		string spentText = spent.ToString().PadLeft(totalLength, '0');
		tex.text = $"Spent: £{spentText}";
	}
}
