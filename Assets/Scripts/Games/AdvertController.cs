using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class AdvertController : MonoBehaviour {
    [SerializeField] private int minTime;
    [SerializeField] private int maxTime;
    [SerializeField] private int repeatExtraDelay;
    [SerializeField] private AudioSource toStop;

    [Header("Internal")]
    [SerializeField] private GameObject videoBackground;
    [SerializeField] private VideoPlayer vid;
    [SerializeField] private RectTransform progressBar;
    [SerializeField] private GameObject downloadNow;


    [HideInInspector] public bool playing { get; private set; }
    private int playTick;
    private bool wasPlaying;

    private void Awake() {
        RandomizeTime(true);
        ;
    }

    private void FixedUpdate() {
        if (vid.isPaused) {
            if (wasPlaying) {
                vid.gameObject.SetActive(false);
                videoBackground.SetActive(false);
                progressBar.gameObject.SetActive(false);
                downloadNow.SetActive(false);

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
                if (!Simulation.menuPopupActive) {
                    playTick--;
                }
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
        downloadNow.SetActive(true);

        UpdateBar();

        vid.time = 0;
        vid.Play();
        RandomizeTime(false);

        wasPlaying = true;
    }

    private void RandomizeTime(bool initial) {
        playTick = Random.Range(minTime, maxTime);
        if (!initial) playTick += repeatExtraDelay;
    }

    private void UpdateBar() {
        Vector3 scale = progressBar.localScale;
        scale.x = (float)(vid.time / vid.length);

        progressBar.localScale = scale;
    }
}
