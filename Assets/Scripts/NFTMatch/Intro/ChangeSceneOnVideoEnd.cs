using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ChangeSceneOnVideoEnd : MonoBehaviour {
    [SerializeField] private string sceneToLoad;
	[SerializeField] private UpdateProps updateSaveProp;
	private enum UpdateProps {
		None,
		Intro
	}
	[SerializeField] private bool reenableClose;

    private VideoPlayer vid;
	private bool done;

	private void Awake() {
		vid = GetComponent<VideoPlayer>();
	}

	private void FixedUpdate() {
		if (vid.isPaused && (! done)) {
			Finish();
		}
	}

	public void Finish() {
		if (updateSaveProp != UpdateProps.None) {
			Save.WatchedClass watched = Simulation.currentSave.watched;
			switch (updateSaveProp) {
				case UpdateProps.Intro:
				{
					watched.intro = true;
					break;
				}
			}
		}

		if (reenableClose) Simulation.preventClose = false;

		Simulation.ChangeScene(sceneToLoad);
		done = true;
	}
}
