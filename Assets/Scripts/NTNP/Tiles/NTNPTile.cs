using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTNPTile : MonoBehaviour {
    [SerializeField] private bool randomizeRotation;

	public void Ready() {
		if (randomizeRotation) {
			transform.Rotate(new Vector3(0, Random.Range(0, 4) * 90, 0));
		}
	}
}
