using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NFTMatchGrid : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject NFTPrefab;
	[SerializeField] private TextAsset gridDataAsset;

	[Header("Misc")]
	[SerializeField] private int size;
	[SerializeField] private float deadzone;

	private GameObject[] NFTGrid;

	private enum Types {
		Red,
		Green,
		Blue,
		Yellow,
		Null
	}
	private Types[] NFTGridData;
	private Types[] startData;
	private int[] NFTPositionIndexes;

	private Vector2 mousePos;
	private Vector2 startPos;
	private Vector2 endPos;

	private int count;

	private void OnPoint(InputValue input) {
		mousePos = Camera.main.ScreenToWorldPoint(input.Get<Vector2>());
	}

	private void OnClick(InputValue input) {
		if (input.isPressed) {
			startPos = mousePos;
		}
		else {
			endPos = mousePos;
			Dragged();
		}
	}


	private void Awake() {
		count = size * size;

		string data = gridDataAsset.text;
		startData = new Types[count];
		for (int i = 0; i < data.Length; i++) {
			startData[i] = (Types)int.Parse(data[i].ToString());
		}

		GenerateGrid();
		RenderGrid();
	}

	private void GenerateGrid() {
		NFTGridData = (Types[])startData.Clone();
		// TODO: rotate and flip randomly (or depending on the number of plays? Use as seed?)
	}
	private void RenderGrid() {
		NFTGrid = new GameObject[count];
		NFTPositionIndexes = new int[count];

		int end = size / 2;
		int start = -end;

		int i = 0;
		for (int y = end; y > start; y--) {
			for (int x = start; x < end; x++) {
				GameObject NFT = Instantiate(NFTPrefab);
				NFT.transform.parent = transform;
				NFT.transform.position = new Vector2(x + 0.5f, y - 0.5f);
				NFTMatchNFT NFTScript = NFT.GetComponent<NFTMatchNFT>();
				NFTScript.type = (NFTMatchNFT.Types)NFTGridData[i];
				NFTScript.id = i;
				NFTScript.Ready();

				NFTGrid[i] = NFT;
				NFTPositionIndexes[i] = i;

				i++;
			}
		}
	}
	
	private int GetPosIndex(int x, int y) {
		if (x < 0 || x >= size) return -1;
		if (y < 0 || y >= size) return -1;

		return (y * size) + x;
	}

	private GameObject GetNFTObByPosIndex(int index) {
		int pos = NFTPositionIndexes[index];
		if (pos == -1) return null;
		return NFTGrid[pos];
	}

	private Types GetTypeAt(int x, int y) {
		int index = GetPosIndex(x, y);
		if (index == -1) return Types.Null;
		index = NFTPositionIndexes[index];
		if (index == -1) return Types.Null; 

		return NFTGridData[index];
	}

	private void DeleteTileID(int id) {
		int pos = NFTPositionIndexes[id];
		if (pos == -1) return;

		GameObject NFT = NFTGrid[pos];
		NFTGridData[pos] = Types.Null;
		NFTPositionIndexes[id] = -1;

		Destroy(NFT);
	}

	private int FallTile(int posIndex) {
		int x = posIndex % size;
		int y = Mathf.FloorToInt(posIndex / size);

		int fallDistance = 0;
		y++;
		while (GetTypeAt(x, y) == Types.Null) {
			y++;
			fallDistance++;
		}
		if (fallDistance == 0) return -1;
		else {
			y--;
			int index = NFTPositionIndexes[posIndex];
			int newPosIndex = GetPosIndex(x, y);

			NFTPositionIndexes[posIndex] = -1;
			NFTPositionIndexes[newPosIndex] = index;

			GameObject NFT = NFTGrid[posIndex];
			NFT.transform.position -= new Vector3(0, fallDistance, 0);
			return newPosIndex;
		}
	}

	private List<int> CheckMatches(int x, int y, List<int> matches) {
		Types type = GetTypeAt(x, y);
		if (type == Types.Null) return matches;

		CheckMatchesSub(x, y, matches, type, new Hashtable());

		return matches;
	}

	private List<int> CheckMatches(int x, int y) {
		return CheckMatches(x, y, new List<int>());
	}

	private void CheckMatchesSub(int x, int y, List<int> matches, Types matchType, Hashtable alreadyDone) {
		if (GetTypeAt(x, y) != matchType) return;
		string key = x + "," + y;
		if ((string)alreadyDone[key] == "1") return;
		alreadyDone[key] = "1";
		matches.Add(GetPosIndex(x, y));

		CheckMatchesSub(x + 1, y, matches, matchType, alreadyDone);
		CheckMatchesSub(x - 1, y, matches, matchType, alreadyDone);
		CheckMatchesSub(x, y + 1, matches, matchType, alreadyDone);
		CheckMatchesSub(x, y - 1, matches, matchType, alreadyDone);
	}

	private void Dragged() {
		Vector2 difference = startPos - endPos;
		if (difference.magnitude < deadzone) return;

		float halfSize = size / 2;
		if (startPos.x > halfSize || startPos.x < -halfSize || startPos.y > halfSize || startPos.y < -halfSize) return;

		Vector2Int[] directions = {
			Vector2Int.left,
			Vector2Int.right,
			Vector2Int.up,
			Vector2Int.down
		};

		int direction;
		if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y)) {
			if (difference.x > 0) {
				direction = 0;
			}
			else {
				direction = 1;
			}
		}
		else {
			if (difference.y > 0) {
				direction = 2;
			}
			else {
				direction = 3;
			}
		}

		int startX = (int)(startPos.x + halfSize);
		int startY = (int)(size - (startPos.y + halfSize));
		int startPosIndex = GetPosIndex(startX, startY);
		if (startPosIndex == -1) return;


		Vector2Int dir = directions[direction];
		int endX = startX + dir.x;
		int endY = startY + dir.y;
		int endPosIndex = GetPosIndex(endX, endY);
		if (endPosIndex == -1) return;

		GameObject startNFT = GetNFTObByPosIndex(startPosIndex);
		GameObject endNFT = GetNFTObByPosIndex(endPosIndex);
		if (startNFT == null || endNFT == null) return;

		NFTPositionIndexes[endPosIndex] = startNFT.GetComponent<NFTMatchNFT>().id;
		NFTPositionIndexes[startPosIndex] = endNFT.GetComponent<NFTMatchNFT>().id;

		bool matched = false;
		List<int> matchIDs = CheckMatches(startX, startY);
		if (matchIDs.Count < 3) {
			matchIDs.Clear();
		}
		else {
			matched = true;
		}
		int countWas = matchIDs.Count;
		CheckMatches(endX, endY, matchIDs);
		int newCount = matchIDs.Count - countWas;
		if (newCount < 3) {
			matchIDs.RemoveRange(countWas, newCount);
		}
		else {
			matched = true;
		}

		if (! matched) { // Revert
			NFTPositionIndexes[endPosIndex] = startNFT.GetComponent<NFTMatchNFT>().id;
			NFTPositionIndexes[startPosIndex] = endNFT.GetComponent<NFTMatchNFT>().id;
			return;
		}

		Vector2 posWas = startNFT.transform.position;
		startNFT.transform.position = endNFT.transform.position;
		endNFT.transform.position = posWas;

		foreach (int id in matchIDs) {
			DeleteTileID(id);
		}


		while (true) {
			List<int> toCheck = new List<int>();
			int start = (NFTGrid.Length - 1) - size; // Skip the bottom row, it can't fall
			for (int i = start; i >= 0; i--) {
				int newID = FallTile(i);
				if (newID != -1) toCheck.Add(newID);
			}

			if (toCheck.Count == 0) break;
			else {
				matchIDs.Clear();
				foreach (int pos in toCheck) {
					int x = pos % size;
					int y = Mathf.FloorToInt(pos / size);

					List<int> newMatches = CheckMatches(x, y);
					if (newMatches.Count > 2) {
						matchIDs.AddRange(newMatches);
					}
				}

				List<string> todo = new List<string>();
				foreach (int pos in toCheck) {
					int x = pos % size;
					int y = Mathf.FloorToInt(pos / size);

					todo.Add(x + "," + y + "," + GetTypeAt(x, y));
				}

				foreach (int id in matchIDs) {
					DeleteTileID(id);
				}
			}
		}

		// TODO: spawn new
	}
}
