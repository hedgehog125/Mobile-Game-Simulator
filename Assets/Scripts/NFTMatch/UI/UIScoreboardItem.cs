using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScoreboardItem : MonoBehaviour {
	private RectTransform tran;
	private TextMeshProUGUI tex;

	private const int scoreLength = 5;

	private void Awake() {
		tran = GetComponent<RectTransform>();
		tex = GetComponent<TextMeshProUGUI>();
	}

	public void Ready(float y, int placement, int score, string username, bool bold) {
		Vector3 pos = tran.anchoredPosition;

		pos.y = y;
		tran.anchoredPosition = pos;

		string scoreText = score.ToString().PadLeft(scoreLength, '0');
		tex.fontStyle = bold? FontStyles.Bold : FontStyles.Normal;
		tex.text = $"{Tools.English.Th(placement)} {username} - {scoreText}";
	}
}
