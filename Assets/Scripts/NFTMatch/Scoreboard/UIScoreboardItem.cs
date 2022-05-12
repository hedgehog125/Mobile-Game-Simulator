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

	[HideInInspector] public float y;
	[HideInInspector] public int placement;
	[HideInInspector] public int score;
	[HideInInspector] public string username;
	[HideInInspector] public bool isPlayer;

	public void UpdateText() {
		Vector3 pos = tran.anchoredPosition;

		pos.y = y;
		tran.anchoredPosition = pos;

		string scoreText = score.ToString().PadLeft(scoreLength, '0');
		tex.fontStyle = isPlayer? FontStyles.Bold : FontStyles.Normal;
		tex.text = $"{Tools.English.Th(placement)} {username} - {scoreText}";
	}
}