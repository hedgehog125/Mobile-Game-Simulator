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
		Yellow
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
		return (y * size) + x;
	}

	private Types GetTypeAt(int x, int y) {
		return NFTGridData[NFTPositionIndexes[GetPosIndex(x, y)]];
	}

	private List<int> CheckMatches(int x, int y, List<int> matches) {
		CheckMatchesSub(x, y, matches, GetTypeAt(x, y));

		return matches;
	}

	private List<int> CheckMatches(int x, int y) {
		return CheckMatches(x, y, new List<int>());
	}

	private void CheckMatchesSub(int x, int y, List<int> matches, Types matchType) {
		if (GetTypeAt(x, y) != matchType) return;
		matches.Add(NFTPositionIndexes[GetPosIndex(x, y)]);

		CheckMatchesSub(x + 1, y, matches, matchType);
		CheckMatchesSub(x - 1, y, matches, matchType);
		CheckMatchesSub(x, y + 1, matches, matchType);
		CheckMatchesSub(x, y - 1, matches, matchType);
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

		Vector2Int dir = directions[direction];
		int endX = startX + dir.x;
		int endY = startY + dir.y;
		int endPosIndex = GetPosIndex(endX, endY);
		if (endPosIndex >= count) { // Screw it, I feel like turning this bug into a feature
			endPosIndex = 0;
		}
		else if (endPosIndex < 0) {
			endPosIndex = count - 1;
		}

		GameObject startNFT = NFTGrid[NFTPositionIndexes[startPosIndex]];
		GameObject endNFT = NFTGrid[NFTPositionIndexes[endPosIndex]];

		List<int> matchIDs = CheckMatches(startX, startY);
		if (matchIDs.Count < 3) return;
		int countWas = matchIDs.Count;
		CheckMatches(endX, endY, matchIDs);
		if (matchIDs.Count - countWas < 3) return;

		foreach (int id in matchIDs) {
			GameObject NFT = NFTGrid[id];

			Destroy(NFT);
		}

		/*
		Vector2 posWas = startNFT.transform.position;
		startNFT.transform.position = endNFT.transform.position;
		endNFT.transform.position = posWas;

		NFTPositionIndexes[endPosIndex] = startNFT.GetComponent<NFTMatchNFT>().id;
		NFTPositionIndexes[startPosIndex] = endNFT.GetComponent<NFTMatchNFT>().id;
		*/
	}
}
