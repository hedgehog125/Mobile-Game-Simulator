using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AdvertController : MonoBehaviour {
    [SerializeField] private int minTime = 5 * 50;
    [SerializeField] private int maxTime = 90 * 50;
    [SerializeField] private int repeatExtraDelay = 30 * 50;
    [SerializeField] private AudioSource toStop;

    private int playTick;
    private int playDelay;
    private VideoPlayer vid;

	private void Awake() {
        vid = GetComponent<VideoPlayer>();
        RandomizeTime(true);
;	}

	private void FixedUpdate() {
		if (playTick == )
	}

	public void Play() {
        if (toStop != null) toStop.Stop();

        vid.Play();
	}

    private void RandomizeTime(bool initial) {
        //playTick = ;
        if (! initial) playTick += repeatExtraDelay;
	}
}
