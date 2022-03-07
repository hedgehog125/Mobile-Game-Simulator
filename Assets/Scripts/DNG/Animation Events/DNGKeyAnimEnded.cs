using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNGKeyAnimEnded : StateMachineBehaviour {
    [SerializeField] private List<string> activateName;

	private ChestController toActivate;
	private void Awake() {
        toActivate = Tools.GetNestedGameobject(activateName).GetComponent<ChestController>();
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.SetActive(false);

        if (toActivate) {
            toActivate.StartAnimation();
        }
    }
}
