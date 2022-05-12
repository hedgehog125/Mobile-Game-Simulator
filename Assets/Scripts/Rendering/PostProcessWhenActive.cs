using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessWhenActive : MonoBehaviour {
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject activeObject;


	private void Update() {
		volume.enabled = activeObject.activeSelf;
	}
}
