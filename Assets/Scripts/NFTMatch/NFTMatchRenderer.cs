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

	private void Init() {
		NFTs = new NFTRenderData[dataScript.count];
		initialized = true;
	}

	public void Rerender() { // Called on update
		if (! initialized) {
			Init();
		}

		// Create any new NFTs that are needed
		for (int i = 0; i < dataScript.count; i++) {
			NFTMatchGrid.GridSquare square = dataScript.grid[i];
			if (square == null) continue;

			Vector2Int xy = dataScript.IndexToXY(i);
			int x = xy.x;
			int y = xy.y + 1;

			if (square.UI_ID == -1) { // New
				GameObject NFT = Instantiate(NFTPrefab);
				int id = FindID();

				NFT.transform.parent = transform;
				NFT.transform.position = new Vector2(x + 0.5f, y - 0.5f);

				NFTMatchNFT NFTScript = NFT.GetComponent<NFTMatchNFT>();
				NFTScript.type = square.type;
				NFTScript.id = id;

				NFTScript.Ready();

				NFTs[id] = new NFTRenderData(NFT);
				square.UI_ID = id;
			}
		}

		Vector2Int[] index = new Vector2Int[dataScript.count];
		for (int i = 0; i < dataScript.count; i++) {
			int id = dataScript.grid[i].UI_ID;

			index[id] = dataScript.IndexToXY(i);
		}

		// Set the target to the corresponding coordinates of where that tile is found now
		for (int i = 0; i < dataScript.count; i++) {
			NFTMatchNFT NFTScript = NFTs[i].script;
			NFTScript.target = index[i];
		}
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
