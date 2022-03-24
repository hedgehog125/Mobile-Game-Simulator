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
	private List<NFTRenderData> NFTs; // The order is unaffected by positions

	private bool initialized;

	[HideInInspector] public bool animating;

	private class NFTFallData {
		public int y;
		public NFTMatchNFT script;

		public NFTFallData(int _y, NFTMatchNFT _script) {
			y = _y;
			script = _script;
		}
	}

	private void Init() {
		NFTs = new List<NFTRenderData>();
		initialized = true;
	}

	private void FixedUpdate() {
		bool someAnimating = false;
		foreach (NFTRenderData NFT in NFTs) {
			if (NFT == null) continue;

			if (NFT.script.animating) {
				someAnimating = true;
			}
		}

		if (! someAnimating) {
			animating = false;
		}
	}

	public void Rerender() { // Called on update
		bool firstInit = false;
		if (! initialized) {
			Init();
			firstInit = true;
		}

		// Create any new NFTs that are needed
		Hashtable justMade = new Hashtable();
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
				NFTScript.dataScript = dataScript;

				NFTScript.Ready();

				NFTs[id] = new NFTRenderData(NFT);
				square.UI_ID = id;
				justMade[id] = "1";
			}
		}

		int[] index = new int[NFTs.Count];
		for (int i = 0; i < dataScript.count; i++) {
			NFTMatchGrid.GridSquare NFT = dataScript.grid[i];
			if (NFT != null) {
				int id = NFT.UI_ID;
				index[id] = i + 1;
			}
		}

		// Set the target to the corresponding coordinates of where that tile is found now
		List<NFTFallData>[] newFallOrder = new List<NFTFallData>[dataScript.pubSize]; // In reverse order
		for (int i = 0; i < NFTs.Count; i++) {
			if (NFTs[i] == null) continue; // Already deleted

			NFTMatchNFT NFTScript = NFTs[i].script;
			int pos = index[i];
			if (pos == 0) { // Wasn't defined, so must have been deleted
				NFTScript.deleted = true; // TODO: also set target when it's one that's been moved
				NFTs[i] = null;
			}
			else {
				NFTScript.ChangeTarget(dataScript.IndexToXY(pos - 1));
				Vector2Int posXY = dataScript.IndexToXY(pos - 1, false); // From 0 to size - 1
				int posX = posXY.x;
				int posY = posXY.y;

				if ((string)justMade[i] == "1") {
					if (newFallOrder[posX] == null) newFallOrder[posX] = new List<NFTFallData>();

					List<NFTFallData> vRow = newFallOrder[posX];
					NFTFallData data = new NFTFallData(posY, NFTScript);
					for (int insertIndex = 0; insertIndex <= vRow.Count; insertIndex++) {
						if (insertIndex == vRow.Count) {
							vRow.Add(data);
							break;
						}
						else if (vRow[insertIndex].y > posY) {
							vRow.Insert(insertIndex, data);
							break;
						}
					}
				}
			}
		}

		if (! firstInit) {
			foreach (List<NFTFallData> vRow in newFallOrder) {
				if (vRow == null) continue;

				int offset = 0;
				for (int i = vRow.Count - 1; i >= 0; i--) {
					vRow[i].script.SetFallOffset(offset);
					offset++;
				}
			}
		}
		animating = true;
	}

	private int FindID() {
		for (int i = 0; i < NFTs.Count; i++) {
			if (NFTs[i] == null) return i;
		}
		NFTs.Add(null);
		return NFTs.Count - 1;
	}
}