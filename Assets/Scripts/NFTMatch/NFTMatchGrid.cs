using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NFTMatchGrid : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private TextAsset gridDataAsset;
	[SerializeField] private NFTMatchRenderer ren;

	[Header("Misc")]
	[SerializeField] private int size;
	[SerializeField] private float deadzone;

	[HideInInspector] public enum SquareType {
		Red,
		Green,
		Blue,
		Yellow,
		Null
	}
	[HideInInspector] public class GridSquare {
		public SquareType type;
		public int UI_ID = -1;

		public GridSquare(SquareType inputType) {
			type = inputType;
			// UI_ID is assigned by the renderer
		}
	}

	private SquareType[] baseGrid;
	[HideInInspector] public GridSquare[] grid { get; private set; }

	private Vector2 mousePos;
	private Vector2 startPos;
	private Vector2 endPos;

	public int pubSize { get; private set; }
	public int count { get; private set; }

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
		pubSize = size;
		count = size * size;

		string data = gridDataAsset.text;
		baseGrid = new SquareType[count];
		for (int i = 0; i < data.Length; i++) {
			SquareType type = (SquareType)int.Parse(data[i].ToString()); 
			baseGrid[i] = type;
		}

		GenerateGrid();
		ren.Rerender();
	}

	private void GenerateGrid() {
		grid = new GridSquare[size * size];
		for (int i = 0; i < baseGrid.Length; i++) {
			grid[i] = new GridSquare(baseGrid[i]);
		}

		// TODO: rotate and flip randomly (or depending on the number of plays? Use as seed?)
	}

	public int GetIndex(int x, int y) {
		if (x < 0 || x >= size) return -1;
		if (y < 0 || y >= size) return -1;

		return (y * size) + x;
	}

	public int[] IndexToXY(int index) {
		int x = index % size;
		int y = Mathf.FloorToInt(index / size);
		int[] xy = {x, y};

		return xy;
	}

	private SquareType GetTypeAt(int x, int y) {
		int index = GetIndex(x, y);
		if (index == -1) return SquareType.Null;

		GridSquare square = grid[index];
		if (square == null) return SquareType.Null;

		return square.type;
	}

	private void DeleteTile(int x, int y) {
		int index = GetIndex(x, y);
		if (index == -1) return;
		DeleteTile(index);
	}
	private void DeleteTile(int index) {
		grid[index] = null;
	}

	public void SwapPair(int index1, int index2) {
		GridSquare tmp = grid[index1];
		grid[index1] = grid[index2];
		grid[index2] = tmp;
	}

	private int FallTile(int posIndex) {
		/*
		int x = posIndex % size;
		int y = Mathf.FloorToInt(posIndex / size);

		int fallDistance = 0;
		y++;
		while (GetTypeAt(x, y) == SquareType.Null) {
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
		*/
		return -1;
	}

	private List<int> CheckMatches(int x, int y, List<int> matches) {
		SquareType type = GetTypeAt(x, y);
		if (type == SquareType.Null) return matches;

		CheckMatchesSub(x, y, matches, type, new Hashtable());

		return matches;
	}

	private List<int> CheckMatches(int x, int y) {
		return CheckMatches(x, y, new List<int>());
	}

	private void CheckMatchesSub(int x, int y, List<int> matches, SquareType matchType, Hashtable alreadyDone) {
		if (GetTypeAt(x, y) != matchType) return;
		string key = x + "," + y;
		if ((string)alreadyDone[key] == "1") return;
		alreadyDone[key] = "1";
		matches.Add(GetIndex(x, y));

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
		int startPosIndex = GetIndex(startX, startY);
		if (startPosIndex == -1) return;


		Vector2Int dir = directions[direction];
		int endX = startX + dir.x;
		int endY = startY + dir.y;
		int endPosIndex = GetIndex(endX, endY);
		if (endPosIndex == -1) return;

		SwapPair(startPosIndex, endPosIndex);

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
			SwapPair(startPosIndex, endPosIndex);
			return;
		}

		foreach (int id in matchIDs) {
			DeleteTile(id);
		}

		/*
		while (true) {
			List<int> toCheck = new List<int>();
			int start = (grid.Length - 1) - size; // Skip the bottom row, it can't fall
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
					DeleteTile(id);
				}
			}
		}
		*/

		// TODO: spawn new

		ren.Rerender();
	}
}
