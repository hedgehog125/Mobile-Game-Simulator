using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class AdvertController : MonoBehaviour {
    [SerializeField] private int minTime = 5 * 50;
    [SerializeField] private int maxTime = 90 * 50;
    [SerializeField] private int repeatExtraDelay = 30 * 50;
    [SerializeField] private AudioSource toStop;

    [Header("Internal")]
    [SerializeField] private GameObject videoBackground;
    [SerializeField] private VideoPlayer vid;
    [SerializeField] private RectTransform progressBar;


    [HideInInspector] public bool playing { get; private set; }
    private int playTick;
    private bool wasPlaying;

	private void Awake() {
        RandomizeTime(true);
;	}

    private void FixedUpdate() {
        if (vid.isPaused) {
            if (wasPlaying) {
                vid.gameObject.SetActive(false);
                videoBackground.SetActive(false);
                progressBar.gameObject.SetActive(false);

                if (toStop != null) toStop.Play();

                wasPlaying = false;
			}
        }

        playing = vid.gameObject.activeSelf;
        if (playing) {
            UpdateBar();
		}
        else {
            if (playTick == 0) {
                Play();
            }
            else {
                playTick--;
            }
        }
	}

	public void Play() {
        if (Simulation.menuPopupActive) return;

        if (toStop != null) toStop.Pause();

        vid.gameObject.SetActive(true);
        vid.time = 0;
        videoBackground.SetActive(true);
        progressBar.gameObject.SetActive(true);
        UpdateBar();

        vid.time = 0;
        vid.Play();
        RandomizeTime(false);

        wasPlaying = true;
	}

    private void RandomizeTime(bool initial) {
        playTick = Random.Range(minTime, maxTime);
        if (! initial) playTick += repeatExtraDelay;
	}

    private void UpdateBar() {
        Vector3 scale = progressBar.localScale;
        scale.x = (float)(vid.time / vid.length);

        progressBar.localScale = scale;
    }
}
