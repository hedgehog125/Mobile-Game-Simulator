using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NFTController : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private List<Sprite> NFTImgAssets;
	[SerializeField] private List<TextAsset> NFTAssets;
	[SerializeField] private SpriteRenderer myRen;
	[SerializeField] private List<string> priceTextPath;
	[SerializeField] private List<string> priceTextPathAlt;

	[Header("Misc")]
	[SerializeField] private int showoffTime;

	[HideInInspector] public int NFT_ID;
	[HideInInspector] public int variationID;
	[HideInInspector] public bool resumeAnimation = true;
	[HideInInspector] public bool canFly { get; private set; }
	[HideInInspector] public int value { get; private set; }

	public class AnimationModes {
		public int showMode;
		public int flyMode;
	}
	[HideInInspector] public AnimationModes animationModes = new AnimationModes();

	private Animator anim;
	private TextMeshProUGUI priceText;
	private GameObject priceTextOb;

	private int showoffTick;

	private void Awake() {
		anim = GetComponent<Animator>();

		GameObject ob = Tools.GetNestedGameobject(priceTextPath);
		if (ob == null) ob = Tools.GetNestedGameobject(priceTextPathAlt);
		if (ob != null) {
			priceTextOb = ob;
			priceText = ob.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
		}
	}

	public Save.DNGSaveClass.OwnedNFT Randomize() {
		Save.DNGSaveClass.OwnedNFT newNFT;
		List<Save.DNGSaveClass.OwnedNFT> notBought = new List<Save.DNGSaveClass.OwnedNFT>();

		for (int id = 0; id < NFTImgAssets.Count; id++) {
			NFTData NFTAsset = JsonUtility.FromJson<NFTData>(NFTAssets[id].text);

			for (int vID = 0; vID < NFTAsset.variations; vID++) {
				newNFT = new Save.DNGSaveClass.OwnedNFT(id, vID);

				if (! Simulation.currentSave.DNGSave.ownedNFTs.Contains(newNFT)) {
					notBought.Add(newNFT);
				}
			}
		}

		newNFT = notBought[Random.Range(0, notBought.Count)];
		NFT_ID = newNFT.baseID;
		variationID = newNFT.variationID;

		return newNFT;
	}

	public void Ready() {
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
		for (int i = 0; i < 4; i++) {
			GameObject pixel = transform.GetChild(i).gameObject;

			pixel.transform.localScale = new Vector3(size, size, 1);
			Vector2 pos = directions[i];

			pixel.transform.localPosition = pos;

			SpriteRenderer ren = pixel.GetComponent<SpriteRenderer>();
			ren.color = new Color(NFT.colors[colorIndex] / 255f, NFT.colors[colorIndex + 1] / 255f, NFT.colors[colorIndex + 2] / 255f);
			ren.enabled = true;
			colorIndex += 3;
		}

		value = NFT.values[variationID];
		if (priceText != null) {
			priceText.text = $"Value: £{value}";
			priceTextOb.SetActive(true);
		}

		anim.SetInteger("ShowMode", animationModes.showMode);
	}

	public void Fly() {
		anim.SetBool("Fly", true);
		anim.SetInteger("FlyMode", animationModes.flyMode);
	}

	private void OnDestroy() {
		if (priceText != null) {
			priceTextOb.SetActive(false);
		}
	}

	private void FixedUpdate() {
		if (! canFly) {
			if (showoffTick == showoffTime) {
				canFly = true;
			}
			else {
				showoffTick++;
			}
		}
	}
}
