using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTFlyDone : StateMachineBehaviour {
    [SerializeField] private List<string> activateName;

    private ChestController toActivate;
    private void Awake() {
        toActivate = Tools.GetNestedGameobject(activateName).GetComponent<ChestController>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (toActivate) {
            toActivate.ResumeAnimation();
        }

        Destroy(animator.gameObject);
    }
}