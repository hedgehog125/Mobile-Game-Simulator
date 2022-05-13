using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDarkenByLights : MonoBehaviour {
    [SerializeField] private List<Light> lights;
	[SerializeField] private float min = 0.2f;
	[SerializeField] private float litAt = 1f;

	private Image img;

	private void Awake() {
		img = GetComponent<Image>();
	}

	private void Update() {
		float total = 0;
		foreach (Light light in lights) {
			if (light.enabled && light.gameObject.activeSelf) {
				total += light.intensity;
			}
		}

		float color = Mathf.Max(total / litAt, min);
		img.color = new Color(color, color, color);
	}
}
