using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIKnowledgePoint : MonoBehaviour {
	private const string repeatMessage = "Ted: You've already found this fact but here it is again...";
	private string[] findMessage = {
		"Ted: Nice work, just give me 5 or 10 minutes...",
		"Ted: Alright, have a read..."
	};

	[Header("Main")]
	[SerializeField] private int knowledgeID;
	[SerializeField] private string title;

	[TextArea(10, 10)]
	[SerializeField] private string fact;
	[SerializeField] private Animator textToGrow;

	[SerializeField] private UIKnowledgePointInner inner;

    private bool outerMouseTouching;
	private bool dialogueWaiting;

	public void OnEnter() {
		outerMouseTouching = true;
	}
	public void OnLeave() {
		outerMouseTouching = false;
	}

	public void OnPointClick(InputValue input) {
		if (! Simulation.revisitingGame) {
			if (input.isPressed) {
				if (outerMouseTouching) {
					if ((! Simulation.menuPopupActive) || Simulation.stayOnLastActive) {
						if (textToGrow != null) {
							textToGrow.SetTrigger("Near");
						}

						if (inner.mouseTouching) {
							Inspect();
						}
					}
				}
			}
		}
	}

	private void Inspect() {
		bool[] pointsGot = Simulation.currentSave.knowledgePointsGot;

		List<string> message = new List<string>();
		if (pointsGot[knowledgeID]) {
			message.Add(repeatMessage);
		}
		else {
			message.Add(findMessage[0]);
			message.Add(findMessage[1]);

			Simulation.currentSave.knowledgePoints++;
			pointsGot[knowledgeID] = true;
		}

		Simulation.textBox.stayOnLast = false;
		Simulation.textBox.Display(message);

		dialogueWaiting = true;
	}

	private void FixedUpdate() {
		if (dialogueWaiting && (! Simulation.menuPopupActive)) {
			Simulation.factBox.Display(title, fact);

			dialogueWaiting = false;
		}
	}
}
