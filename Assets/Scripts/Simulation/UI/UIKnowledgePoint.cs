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

		if (Simulation.currentSave.knowledgePointsGot[knowledgeID]) {
			message.Insert(0, "Ted: You've already found this fact but here it is again...");
		}


		Display(message);
	}

	private void Display(List<string> message) {

	}
}
