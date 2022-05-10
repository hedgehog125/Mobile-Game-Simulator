using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnowledgePointMisc : MonoBehaviour {
    private static List<string> revisitMessage;
    private static List<string> gotAllMessage;

    static KnowledgePointMisc() {
        revisitMessage = new List<string>();
        revisitMessage.Add("Ted: The time's up for this game, so you can't find any more malpractices");

        gotAllMessage = new List<string>();
        gotAllMessage.Add("Ted: You've already found everything in this game, try another game");
    }

    private void OnPointClick(InputValue input) {
        if (Simulation.revisitingGame) {
            if (input.isPressed) {
                if (! Simulation.menuPopupActive) {
                    Simulation.textBox.Display(Simulation.gotAllInGame? gotAllMessage : revisitMessage);
				}
		    }
		}
	}
}
