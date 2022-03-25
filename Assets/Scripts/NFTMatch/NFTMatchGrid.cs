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

	[HideInInspector] public static int squareTypeCount = 6;
	[HideInInspector] public enum SquareType {
		Red,
		Green,
		Blue,
		Yellow,
		Pink,
		SkyBlue,
		Null
	}
	[HideInInspector] public class GridSquare {
		public SquareType type;
		public int UI_ID = -1;

		public GridSquare() {
			type = (SquareType)(Random.Range(0, squareTypeCount - 1));
		}
		public GridSquare(SquareType inputType) {
			type = inputType;
			// UI_ID is assigned by the renderer
		}
	}

	private SquareType[] baseGrid;
	[HideInInspector] public GridSquare[] grid { get; private set; }

	[HideInInspector] public class SwappedSquare {
		public int UI_ID;
		public Vector2Int dir;

		public SwappedSquare(int _UI_ID, Vector2Int _dir) {
			UI_ID = _UI_ID;
			dir = _dir;
		}
	}

	private Vector2 mousePos;
	private Vector2 startPos;
	private Vector2 endPos;
	private class MoveQueueItem {
		public Vector2Int start;
		public int startIndex;
		public Vector2Int end;
		public int endIndex;
		public Vector2Int dir;
		public bool isCheck;
		public bool isSeparator;

		public MoveQueueItem(
			Vector2Int _start,
			int _startIndex,
			Vector2Int _end,
			int _endIndex,
			Vector2Int _dir
		) {
			start = _start;
			startIndex = _startIndex;
			end = _end;
			endIndex = _endIndex;
			dir = _dir;
			isCheck = false;
		}
		public MoveQueueItem( // Check
			Vector2Int _start,
			int _startIndex
		) {
			start = _start;
			startIndex = _startIndex;
			isCheck = true;
		}
		public MoveQueueItem() { // Separator
			isSeparator = true;
		}
	}
	private List<MoveQueueItem> queue;

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
			QueueDragged();
		}
	}


	private void Awake() {
		pubSize = size;
		count = size * size;

		string data = gridDataAsset.text;
		baseGrid = new SquareType[count];
		for (int i = 0; i < data.Length; i++) {
			int typeID = int.Parse(data[i].ToString());
			baseGrid[i] = (SquareType)typeID;
		}

		queue = new List<MoveQueueItem>();

		GenerateGrid();
		ren.Rerender();
	}

	private void FixedUpdate() {
		if (! ren.animating) {
			ProcessDragQueue();
		}
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

	public Vector2Int IndexToXY(int index) {
		return IndexToXY(index, true);
	}
	public Vector2Int IndexToXY(int index, bool worldSpace) {
		int x = index % size;
		int y = Mathf.FloorToInt(index / size);
		if (worldSpace) {
			x -= size / 2;
			y = (size / 2) - y;
		}

		return new Vector2Int(x, y);
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
		int x = posIndex % size;
		int y = Mathf.FloorToInt(posIndex / size);

		int fallDistance = 0;
		y++;
		while (GetTypeAt(x, y) == SquareType.Null) {
			y++;
			fallDistance++;
			if (y == size) break;
		}
		if (fallDistance == 0) return -1;

		int newPosIndex = GetIndex(x, y - 1);
		SwapPair(posIndex, newPosIndex);

		return newPosIndex;
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

	private void QueueDragged() {
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

		queue.Add(new MoveQueueItem(
			new Vector2Int(startX, startY),
			startPosIndex,
			new Vector2Int(endX, endY),
			endPosIndex,
			dir
		));
		queue.Add(new MoveQueueItem()); // Separator
	}

	private void ProcessDragQueue() {
		SwappedSquare[] swappedSquares = new SwappedSquare[2];

		bool isCheck = false;
		bool needsRender = false;
		do {
			if (queue.Count == 0) break;
			MoveQueueItem queuedItem = queue[0];
			if (queuedItem.isSeparator) {
				queue.RemoveAt(0);
				if (ren.animating) break;
				continue; // The next batch can be done a frame early since nothing changed (this also shouldn't include another batch of checks since the grid wasn't changed)
			}

			if (ProcessDragQueueItem(queuedItem, out isCheck, swappedSquares)) {
				needsRender = true;
			}
			queue.RemoveAt(0);
		} while (isCheck); // Multiple checks are usually run after every drag. They're run in batches and any new checks that get queued are done in the next batch due to the separator

		if (needsRender) {
			ren.Rerender(swappedSquares);
		}
	}

	private bool ProcessDragQueueItem(MoveQueueItem queuedItem, out bool isCheck, SwappedSquare[] swappedSquares) {
		int startX = queuedItem.start.x;
		int startY = queuedItem.start.y;
		int startPosIndex = queuedItem.startIndex;

		int endX = queuedItem.end.x;
		int endY = queuedItem.end.y;
		int endPosIndex = queuedItem.endIndex;
		isCheck = queuedItem.isCheck;

		if (! isCheck) {
			SwapPair(startPosIndex, endPosIndex);
		}

		bool matched = false;
		List<int> matchIDs = CheckMatches(startX, startY);
		if (matchIDs.Count < 3) {
			matchIDs.Clear();
		}
		else {
			matched = true;
		}

		if (! isCheck) {
			int countWas = matchIDs.Count;
			CheckMatches(endX, endY, matchIDs);
			int newCount = matchIDs.Count - countWas;
			if (newCount < 3) {
				matchIDs.RemoveRange(countWas, newCount);
			}
			else {
				matched = true;
			}
		}

		if (! matched) { // Revert
			if (! isCheck) {
				SwapPair(startPosIndex, endPosIndex);
			}
			return false;
		}

		if (! isCheck) {
			Vector2Int startDir = new Vector2Int(endX - startX, startY - endY); // Y is flipped for worldspace vs grid-space
			Vector2Int endDir = startDir * -Vector2Int.one;

			swappedSquares[0] = new SwappedSquare(grid[endPosIndex].UI_ID, startDir);
			swappedSquares[1] = new SwappedSquare(grid[startPosIndex].UI_ID, endDir);
		}

		foreach (int id in matchIDs) {
			DeleteTile(id);
		}

		// Falling

		int start = (grid.Length - 1) - size; // Skip the bottom row, it can't fall
		for (int i = start; i >= 0; i--) {
			int newID = FallTile(i);
			if (newID != -1) {
				queue.Add(new MoveQueueItem(IndexToXY(newID, false), newID));
			}
		}

		// Spawn replacements
		for (int i = 0; i < count; i++) {
			if (grid[i] == null) {
				grid[i] = new GridSquare();
				queue.Add(new MoveQueueItem(IndexToXY(i, false), i));
			}
		}
		queue.Add(new MoveQueueItem()); // Separator

		return true;
	}
}