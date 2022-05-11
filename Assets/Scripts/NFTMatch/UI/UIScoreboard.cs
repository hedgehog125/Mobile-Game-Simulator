using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScoreboard : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private GameObject scorePrefab;

	[Header("Board")]
	[SerializeField] private int topScore;
	[SerializeField] private int scoreGap; // So there'll be one 'opponent' to beat for every tile you match

	[Header("Display")]
    [SerializeField] private int countAtOnce;
	[SerializeField] private int outerPadding;
	[SerializeField] private int maxNameLength;

	private RectTransform tran;
	private float rowHeight;
	private float topY;
	private int totalCount;

	private void Awake() {
		tran = GetComponent<RectTransform>();

		topY = tran.rect.height - (outerPadding * 2);
		rowHeight = topY / countAtOnce;
		topY = (topY / 2) - (rowHeight / 2);

		totalCount = topScore / scoreGap;

		int score = (countAtOnce - 1) * scoreGap;
		float y = topY;
		int placement = totalCount - (countAtOnce + 1);
		for (int i = 0; i < countAtOnce - 1; i++) {
			NewItem(y, placement, score, Tools.Random.Username(maxNameLength), false);
			y -= rowHeight;
			placement++;
			score -= scoreGap;
		}

		NewItem(y, placement, 0, "You", true);
	}

	private void NewItem(float y, int placement, int score, string username, bool bold) {
		GameObject item = Instantiate(scorePrefab, transform);
		UIScoreboardItem script = item.GetComponent<UIScoreboardItem>();

		script.Ready(y, placement, score, username, bold);
	}
}
