using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTNPDeathPlaneController : MonoBehaviour {
	[Header("Objects and References")]
	[SerializeField] private GameObject centerCube;

	[Header("Difficulty and Speed")]
	[SerializeField] private float initialSpeed;
	[SerializeField] private float acceleration;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float speedUpPointOffset;
	[SerializeField] private float maxFollowSpeed;

	private float speed;
	private GameObject playerObject;
	private PlayerDeath deathScript; 

	private void Awake() {
		playerObject = GameObject.Find("Player");
		deathScript = playerObject.GetComponent<PlayerDeath>();

		speed = initialSpeed;
	}

	private void Update() {
		Vector3 pos = transform.position;
		if (deathScript.alive) {
			pos.z += speed * Time.deltaTime;
			pos.z += Mathf.Clamp(playerObject.transform.position.z - (centerCube.transform.position.z + speedUpPointOffset), 0, maxFollowSpeed * Time.deltaTime);
			transform.position = pos;

			speed += acceleration * Time.deltaTime;
			if (speed > maxSpeed) speed = maxSpeed;
		}
	}
}