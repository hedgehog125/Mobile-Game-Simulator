using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTFlyDone : StateMachineBehaviour {
    [SerializeField] private List<string> activateName;

    private Animator toActivate;
    private void Awake() {
        toActivate = Tools.GetNestedGameobject(activateName).GetComponent<Animator>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (toActivate) {
            toActivate.enabled = true;
        }

        Destroy(animator.gameObject);
    }
}
