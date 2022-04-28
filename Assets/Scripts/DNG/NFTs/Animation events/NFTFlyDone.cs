using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class NFTFlyDone : StateMachineBehaviour {
    [SerializeField] private List<string> activateName;

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        NFTController myController = animator.gameObject.GetComponent<NFTController>();

        if (myController.resumeAnimation) {
            ChestController toActivate = Tools.GetNestedGameobject(activateName).GetComponent<ChestController>();
            toActivate.ResumeAnimation();
        }

        Destroy(animator.gameObject);
    }
}
