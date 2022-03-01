using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTController : MonoBehaviour {
	[SerializeField] private List<Sprite> NFTImgAssets;
	[SerializeField] private List<TextAsset> NFTAssets;
	[SerializeField] private int NFT_ID;
	[SerializeField] private int variationID;

	private void Awake() {
		SpriteRenderer myRen = GetComponent<SpriteRenderer>();
		myRen.sprite = NFTImgAssets[NFT_ID];
		Bounds bounds = myRen.bounds;
		NFTData NFT = JsonUtility.FromJson<NFTData>(NFTAssets[NFT_ID].text);

		float scale = transform.localScale.x;
		float size = NFT.size;
		float halfSize = size / 2;
		int colorIndex = variationID * (4 * 3);

		Vector2[] directions = {
			new Vector2((bounds.min.x / scale) + halfSize, (bounds.min.y / scale) + halfSize),
			new Vector2((bounds.max.x / scale) - halfSize, (bounds.min.y / scale) + halfSize),
			new Vector2((bounds.min.x / scale) + halfSize, (bounds.max.y / scale) - halfSize),
			new Vector2((bounds.max.x / scale) - halfSize, (bounds.max.y / scale) - halfSize)
		};
		for (int i = 0; i < transform.childCount; i++) {
			GameObject pixel = transform.GetChild(i).gameObject;

			pixel.transform.localScale = new Vector3(size, size, 1);
			Vector2 pos = directions[i];

			pixel.transform.localPosition = pos;

			SpriteRenderer ren = pixel.GetComponent<SpriteRenderer>();
			ren.color = new Color(NFT.colors[colorIndex] / 255f, NFT.colors[colorIndex + 1] / 255f, NFT.colors[colorIndex + 2] / 255f);
			ren.enabled = true;
			colorIndex += 3;
		}
	}
}
