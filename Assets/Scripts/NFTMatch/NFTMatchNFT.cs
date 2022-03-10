using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchNFT : MonoBehaviour {
    [HideInInspector] public enum Types {
		Red,
		Green,
		Blue,
		Yellow
	}
	[HideInInspector] public Types type;

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
