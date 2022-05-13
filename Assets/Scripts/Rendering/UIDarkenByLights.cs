using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDarkenByLights : MonoBehaviour {
    [SerializeField] private List<Light> lights;
	[SerializeField] private float min = 0.2f;
	[SerializeField] private float litAt = 1f;
	[SerializeField] private float darknessEffect = 1f;

	[Header("Angle handling")]
	[SerializeField] private Angles directAngleType;

	private enum Angles {
		x,
		y,
		z
	}
	[SerializeField] private float directAngle;
	[SerializeField] private float fullyIndirectOffset = 90;

	[Header("Debug")]
	public float measuredLight;

	private Image img;

	private void Awake() {
		img = GetComponent<Image>();
	}

	private void Update() {
		float total = 0;
		foreach (Light light in lights) {
			if (light.enabled && light.gameObject.activeSelf) {
				float directness = 1;
				if (light.type == LightType.Spot || light.type == LightType.Directional) {
					float angle;
					if (directAngleType == Angles.x) {
						angle = light.transform.eulerAngles.x;
					}
					else if (directAngleType == Angles.x) {
						angle = light.transform.eulerAngles.y;
					}
					else {
						angle = light.transform.eulerAngles.z;
					}

					float difference = Mathf.DeltaAngle(angle, directAngle);

					directness = 1 - (difference / fullyIndirectOffset);
					if (directness < 0) directness = 0;
				}

				total += light.intensity * directness;
			}
		}

		measuredLight = total;
		float color = Mathf.Max(total / litAt, min);
		color = 1 - ((1 - color) * darknessEffect);

		img.color = new Color(color, color, color);
	}
}
