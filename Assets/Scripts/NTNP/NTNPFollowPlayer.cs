using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTNPFollowPlayer : MonoBehaviour {
    private GameObject playerObject;

	private void Awake() {
		playerObject = GameObject.Find("Player");
	}

	private void Update() {
		Vector3 pos = transform.position;
		pos.x = playerObject.transform.position.x;

		transform.position = pos;
	}
}
