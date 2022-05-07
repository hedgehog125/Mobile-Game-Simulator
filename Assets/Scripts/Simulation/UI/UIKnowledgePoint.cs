using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIKnowledgePoint : MonoBehaviour {
	[SerializeField] private int knowledgeID;
	[SerializeField] private string title;

	[TextArea(10, 10)]
	[SerializeField] private string fact;

    private bool mouseTouching;
	private bool dialogueWaiting;

	public void OnEnter() {
		mouseTouching = true;
	}
	public void OnLeave() {
		mouseTouching = false;
	}

	public void OnPointClick(InputValue input) {
		if (input.isPressed && mouseTouching) {
			Inspect();
		}
	}

	private void Inspect() {
		if (Simulation.menuPopupActive) return;

		bool[] pointsGot = Simulation.currentSave.knowledgePointsGot;

		List<string> message = new List<string>();
		if (pointsGot[knowledgeID]) {
			message.Add("Ted: You've already found this fact but here it is again...");
		}
		else {
			message.Add("Ted: Nice work, one second... *frantically researches and writes*");
			message.Add("Ted: Tada...");

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
