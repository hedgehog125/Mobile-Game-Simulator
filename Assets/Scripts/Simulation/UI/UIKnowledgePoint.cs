using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIKnowledgePoint : MonoBehaviour {
	[SerializeField] private int knowledgeID;
	[SerializeField] private List<string> fact;

    private bool mouseTouching;

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
		List<string> message = new List<string>(fact);

		bool[] pointsGot = Simulation.currentSave.knowledgePointsGot;

		if (pointsGot[knowledgeID]) {
			message.Insert(0, "Ted: You've already found this fact but here it is again...");
		}
		else {
			Simulation.currentSave.knowledgePoints++;
			pointsGot[knowledgeID] = true;
		}

		Simulation.textBox.Display(message);
	}
}
