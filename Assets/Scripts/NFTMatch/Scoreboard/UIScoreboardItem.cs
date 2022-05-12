using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIScoreboardItem : MonoBehaviour {
	[SerializeField] private TextMeshProUGUI tex;
	[SerializeField] private TextMeshProUGUI scoreText;

	private RectTransform tran;
	private Image img;
	private Outline outln;

	private const int scoreLength = 5;

	private void Awake() {
		tran = GetComponent<RectTransform>();
		img = GetComponent<Image>();
		outln = GetComponent<Outline>();
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

		tex.text = $"{Tools.English.Th(placement)} {username}";

		scoreText.fontStyle = tex.fontStyle;
		scoreText.text = score.ToString().PadLeft(scoreLength, '0');

		if (isPlayer) {
			img.enabled = true;
			outln.enabled = true;
		}
		else {
			img.enabled = false;
			outln.enabled = false;
		}
	}
}
