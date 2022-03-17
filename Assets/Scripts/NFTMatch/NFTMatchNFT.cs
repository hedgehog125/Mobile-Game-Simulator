using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchNFT : MonoBehaviour {
	[HideInInspector] public NFTMatchGrid.SquareType type;
	[HideInInspector] public int id;

	private SpriteRenderer ren;

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
}
