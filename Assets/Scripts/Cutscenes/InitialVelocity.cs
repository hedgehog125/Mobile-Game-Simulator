using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVelocity : MonoBehaviour {
    [SerializeField] Vector3 speed;

	private void Awake() {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.velocity = speed;
	}
}
