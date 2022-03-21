using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchRenderer : MonoBehaviour {
	[SerializeField] private GameObject NFTPrefab;
    [SerializeField] private NFTMatchGrid dataScript;

	private class NFTRenderData {
		public Rigidbody2D rb;
		public NFTMatchNFT script;

		public NFTRenderData(GameObject NFT) {
			rb = NFT.GetComponent<Rigidbody2D>();
			script = NFT.GetComponent<NFTMatchNFT>();
		}
	}
	private NFTRenderData[] NFTs; // The order is unaffected by positions

	private bool initialized;

	[HideInInspector] public bool animating;

	private void Init() {
		NFTs = new NFTRenderData[dataScript.count];
		initialized = true;
	}

	private void FixedUpdate() {
		animating = false;
	}

	public void Rerender() { // Called on update
		if (! initialized) {
			Init();
		}

		// Create any new NFTs that are needed
		for (int i = 0; i < dataScript.count; i++) {
			NFTMatchGrid.GridSquare square = dataScript.grid[i];
			if (square == null) continue;

			if (square.UI_ID == -1) { // New
				GameObject NFT = Instantiate(NFTPrefab);
				int id = FindID();

				NFT.transform.parent = transform;
				NFTMatchNFT NFTScript = NFT.GetComponent<NFTMatchNFT>();
				NFTScript.type = square.type;
				NFTScript.id = id;

				NFTScript.Ready();

				NFTs[id] = new NFTRenderData(NFT);
				square.UI_ID = id;
			}
		}

		int[] index = new int[dataScript.count];
		for (int i = 0; i < dataScript.count; i++) {
			NFTMatchGrid.GridSquare NFT = dataScript.grid[i];
			if (NFT != null) {
				int id = NFT.UI_ID;
				index[id] = i + 1;
			}
		}

		// Set the target to the corresponding coordinates of where that tile is found now
		for (int i = 0; i < dataScript.count; i++) {
			NFTMatchNFT NFTScript = NFTs[i].script;
			int pos = index[i];
			if (pos == 0) { // Wasn't defined, so must have been deleted
				NFTScript.deleted = true;
			}
			else {
				NFTScript.target = dataScript.IndexToXY(pos - 1);
			}
		}
		animating = true;
	}

	private int FindID() {
		for (int i = 0; i < NFTs.Length; i++) {
			if (NFTs[i] == null) return i;
		}
		return -1;
	}

	//private int[] Index() {

	//}
}
