using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour {
    [SerializeField] private Animator animator;
    [SerializeField] private NTNPMovement moveScript;

	private bool wasTurning;

	private void FixedUpdate() {
		if (moveScript.movingDirection == NTNPMovement.Directions.None) {
			animator.SetBool("Walking", false);
		}
		else {
			animator.SetBool("Walking", true);
		}


		// TODO: call if turnAmount changes
		bool turning = moveScript.turnAmount != 0;
		if (wasTurning && (! turning)) {
			moveScript.OnRotateAnimationFinished();
		}
		wasTurning = turning;
		animator.SetInteger("Turn", moveScript.turnAmount);
	}
}
