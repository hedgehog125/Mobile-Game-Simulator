using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTController : MonoBehaviour {
	[SerializeField] private TextAsset NFTAsset;
	[SerializeField] private int variationID;

	private void Awake() {
		Bounds bounds = GetComponent<SpriteRenderer>().bounds;
		NFTData NFT = JsonUtility.FromJson<NFTData>(NFTAsset.text);

		float scale = transform.localScale.x;
		float size = NFT.size;
		float halfSize = size / 2;
		int colorIndex = variationID * (4 * 3);
		for (int i = 0; i < transform.childCount; i++) {
			GameObject pixel = transform.GetChild(i).gameObject;

			pixel.transform.localScale = new Vector3(size, size, 1);
			Vector2 pos;
			if (i == 0) {
				pos = new Vector2((bounds.min.x / scale) + halfSize, (bounds.min.y / scale) + halfSize);
			}
			else if (i == 1) {
				pos = new Vector2((bounds.max.x / scale) - halfSize, (bounds.min.y / scale) + halfSize);
			}
			else if (i == 2) {
				pos = new Vector2((bounds.min.x / scale) + halfSize, (bounds.max.y / scale) - halfSize);
			}
			else {
				pos = new Vector2((bounds.max.x / scale) - halfSize, (bounds.max.y / scale) - halfSize);
			}
			pixel.transform.localPosition = pos;

			SpriteRenderer ren = pixel.GetComponent<SpriteRenderer>();
			ren.color = new Color(NFT.colors[colorIndex] / 255f, NFT.colors[colorIndex + 1] / 255f, NFT.colors[colorIndex + 2] / 255f);
			ren.enabled = true;
			colorIndex += 3;
		}
	}
}
