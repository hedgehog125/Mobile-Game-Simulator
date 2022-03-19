using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchNFT : MonoBehaviour {
	[HideInInspector] public NFTMatchGrid.SquareType type;
	[HideInInspector] public int id;

	private SpriteRenderer ren;

	[HideInInspector] public Vector2 target;
	[HideInInspector] public bool deleted;

	private void Awake() {
		ren = GetComponent<SpriteRenderer>();
		Position();
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
		Position();
	}

	private void Position() {
		transform.position = target + new Vector2(0.5f, -0.5f);
		if (deleted) {
			Destroy(gameObject);
			return;
		}
	}
}
