using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchNFT : MonoBehaviour {
	[HideInInspector] public NFTMatchGrid.SquareType type;
	[HideInInspector] public int id;

	private SpriteRenderer ren;

	[HideInInspector] public Vector2 target;

	private void Awake() {
		ren = GetComponent<SpriteRenderer>();
	}

	public void Ready() {
		Color[] colors = {
			Color.red,
			Color.green,
			Color.blue,
			Color.yellow
		};

		ren.color = colors[(int)type];
	}

	private void Update() {
		transform.position = target;
	}
}
