using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTNPOffScreenDetector : MonoBehaviour {
	[SerializeField] private float length;

    private GameObject deathPlane;

    private void Awake() {
        deathPlane = GameObject.Find("DeathPlane");
    }

    private void FixedUpdate() {
        if (transform.position.z + (length / 2) < deathPlane.transform.position.z) {
            Destroy(gameObject);
        }
    }
}