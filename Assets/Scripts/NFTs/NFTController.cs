using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTController : MonoBehaviour {
	[SerializeField] private List<Texture2D> NFTImgAssets;
	[SerializeField] private List<TextAsset> NFTAssets;

	[HideInInspector] public int NFT_ID;
	[HideInInspector] public int variationID;

	[HideInInspector] public bool animate;

	public void Randomize() {

	}

	public void Ready() {
		Renderer myRen = GetComponent<Renderer>();
		myRen.material.mainTexture = NFTImgAssets[NFT_ID];
		if (animate) {
			Animator anim = GetComponent<Animator>();
			anim.enabled = true;
		}

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
		for (int i = 0; i < 4; i++) {
			GameObject pixel = transform.GetChild(i).gameObject;

			pixel.transform.localScale = new Vector3(size, size, 1);
			Vector2 pos = directions[i];

			pixel.transform.localPosition = pos;

			Renderer ren = pixel.GetComponent<Renderer>();
			ren.material.color = new Color(NFT.colors[colorIndex] / 255f, NFT.colors[colorIndex + 1] / 255f, NFT.colors[colorIndex + 2] / 255f);
			ren.enabled = true;
			colorIndex += 3;
		}
	}
}
