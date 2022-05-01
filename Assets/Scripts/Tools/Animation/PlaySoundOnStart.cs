using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : StateMachineBehaviour {
    [SerializeField] private int audioID;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        AudioSource[] audios = animator.gameObject.GetComponents<AudioSource>();
        audios[audioID].Play();
    }
}
