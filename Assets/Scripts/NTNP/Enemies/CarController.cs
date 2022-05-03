using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {
    [SerializeField] private bool direction;

	private float speed = 2;

	private void Awake() {
		if (direction) {
			transform.localScale = new Vector3(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z);
		}
	}

	private void Update() {
		Vector3 position = transform.position;
		float amount = speed * Time.deltaTime;
		position.x += direction? amount : -amount;

		transform.position = position;
	}
}
