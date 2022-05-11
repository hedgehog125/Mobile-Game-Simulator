using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardRenderer : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject scorePrefab;

	[Header("")]
	[SerializeField] private int countAtOnce;
	[SerializeField] private int outerPadding;


	private RectTransform tran;
	private float rowHeight;
	private float topY;

	private bool renderedBefore;
	private UIScoreboardItem[] items;
	private int cameraY;

	private void Awake() {
		tran = GetComponent<RectTransform>();

		topY = tran.rect.height - (outerPadding * 2);
		rowHeight = topY / countAtOnce;
		topY = (topY / 2) - (rowHeight / 2);
	}

	public void Render(ScoreboardController.ScoreboardItem[] scoreboard, int playerScore) {
		int playerPlacement = scoreboard.Length;
		while (playerPlacement != 0 && scoreboard[playerPlacement - 1].score < playerScore) {
			playerPlacement--;
		}

		cameraY = (playerPlacement - countAtOnce) + 1;
		if (cameraY < 0) cameraY = 0;

		if (! renderedBefore) {
			items = new UIScoreboardItem[countAtOnce];

			float y = topY;
			for (int i = 0; i < countAtOnce; i++) {
				GameObject item = Instantiate(scorePrefab, transform);
				UIScoreboardItem script = item.GetComponent<UIScoreboardItem>();

				script.y = y;

				items[i] = script;
				y -= rowHeight;
			}
		}

		bool passedPlayer = false;
		for (int i = 0; i < countAtOnce; i++) {
			int c = i + cameraY;
			UIScoreboardItem script = items[i];

			script.placement = c;
			if (c == playerPlacement) {
				script.score = playerScore;
				script.username = "You";
				script.isPlayer = true;

				passedPlayer = true;
			}
			else {
				if (passedPlayer) c--; // So that player isn't skipped over
				ScoreboardController.ScoreboardItem score = scoreboard[c];

				script.score = score.score;
				script.username = score.username;
				script.isPlayer = false;
			}

			script.UpdateText();
		}


		renderedBefore = true;
	}
}
