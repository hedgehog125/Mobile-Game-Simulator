using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchRenderer : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject NFTPrefab;
	[SerializeField] private NFTMatchGrid dataScript;

	[Header("Misc")]
	[SerializeField] private float baseSpeed;
	[SerializeField] private float startSpeedIncrease;
	[SerializeField] private float maxStartSpeed;
	[SerializeField] private float fallAcceleration;
	[SerializeField] private float maxSpeed;

	[HideInInspector] public float speed { get; private set; }
	[HideInInspector] public float fallSpeed { get; private set; }
	private float baseSpeedCounteracted;

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
	private bool firstInit = true;
	private Hashtable justMade;

	[HideInInspector] public bool animating { get; private set; }
	private bool swapAnimating;
	private int justFinishedTick;

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

		baseSpeedCounteracted = baseSpeed - startSpeedIncrease; // It gets increased even for a single batch of matches so it needs counteracting
		speed = baseSpeedCounteracted;
		fallSpeed = baseSpeed;
	}

	private void FixedUpdate() {
		if (animating) {
			bool someAnimating = false;
			foreach (NFTRenderData NFT in NFTs) {
				if (NFT == null) continue;

				if (NFT.script.animating) {
					someAnimating = true;
				}
			}

			fallSpeed = Mathf.Min(fallSpeed + (fallAcceleration / 50), maxSpeed);
			justFinishedTick = 0;
			if (! someAnimating) {
				if (swapAnimating) {
					AfterSwap();
					swapAnimating = false;
				}
				else {
					animating = false;
					justFinishedTick = 1;
				}
			}
		}
		else {
			fallSpeed = baseSpeed;
			if (justFinishedTick != 0) {
				if (justFinishedTick == 2) {
					justFinishedTick = 0;
					speed = baseSpeedCounteracted;
				}
				else {
					justFinishedTick++;
				}
			}
		}
	}

	private void AfterSwap() {
		int[] index = new int[NFTs.Count];
		for (int i = 0; i < dataScript.count; i++) {
			NFTMatchGrid.GridSquare NFT = dataScript.grid[i];
			if (NFT != null) {
				int id = NFT.UI_ID;
				index[id] = i + 1;
			}
		}

		// Set the target to the corresponding coordinates of where that tile is found now
		List<NFTFallData>[] newFallOrder = new List<NFTFallData>[dataScript.size]; // In reverse order
		for (int i = 0; i < NFTs.Count; i++) {
			if (NFTs[i] == null) continue; // Already deleted

			NFTMatchNFT NFTScript = NFTs[i].script;
			int pos = index[i];
			if (pos == 0) { // Wasn't defined, so must have been deleted
				NFTScript.deleted = true;
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

		firstInit = false;
	}

	public void Rerender() { // In theory, this will only ever be called in this form on a first init, in which case, the value isn't needed
		Rerender(null);
	}

	public void Rerender(NFTMatchGrid.SwappedSquare[] swappedSquares) { // Called on update
		speed = Mathf.Min(speed + startSpeedIncrease, maxStartSpeed);

		firstInit = false;
		if (! initialized) {
			Init();
			firstInit = true;
		}

		// Create any new NFTs that are needed
		justMade = new Hashtable();
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
				NFTScript.parentScript = this;

				NFTScript.Ready(); // It won't be shown until the target is set though

				NFTs[id] = new NFTRenderData(NFT);
				square.UI_ID = id;
				justMade[id] = "1";
			}
		}

		animating = true;
		if (firstInit) {
			swapAnimating = false;
			AfterSwap();
		}
		else {
			swapAnimating = true;

			SetSwappedTarget(swappedSquares[0]);
			SetSwappedTarget(swappedSquares[1]);
		}
	}

	private void SetSwappedTarget(NFTMatchGrid.SwappedSquare swappedSquare) {
		if (swappedSquare == null) return;
		NFTRenderData NFT = NFTs[swappedSquare.UI_ID];
		if (NFT == null) return;

		NFT.script.ChangeTargetDir(swappedSquare.dir);
		NFT.script.swapping = true;
	}

	private int FindID() {
		for (int i = 0; i < NFTs.Count; i++) {
			if (NFTs[i] == null) return i;
		}
		NFTs.Add(null);
		return NFTs.Count - 1;
	}
}