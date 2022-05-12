using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteOnAnimationFinish : StateMachineBehaviour {
    [SerializeField] private bool destroyParent;
    [SerializeField] private bool onlyDeactivate;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        GameObject toDestroy;
        if (destroyParent) {
            toDestroy = animator.transform.parent.gameObject;
		}
        else {
            toDestroy = animator.gameObject;
		}

        if (onlyDeactivate) {
            toDestroy.SetActive(false);
		}
        else {
            Destroy(toDestroy);
		}
    }
}
