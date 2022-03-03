using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNGKeyAnimEnded : StateMachineBehaviour {
    [SerializeField] private List<string> activateName;

	private Animator toActivate;
	private void Awake() {
        Transform activateTransform = GameObject.Find(activateName[0]).transform;
        for (int i = 1; i < activateName.Count; i++) {
            activateTransform = activateTransform.Find(activateName[i]);
        }

        toActivate = activateTransform.gameObject.GetComponent<Animator>();
	}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.gameObject.SetActive(false);
        if (toActivate) {
            toActivate.enabled = true;
		}
    }
}
