using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    [HideInInspector] public bool alive { get; private set; } = true;

	private Renderer ren;
	private void Awake() {
		ren = transform.GetChild(0).gameObject.GetComponent<Renderer>();
	}

	private void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.CompareTag("DeathPlane")) {
			if (alive) {
				alive = false;
				ren.enabled = false;
			}
		}
	}
}
