using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardController : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private ScoreboardRenderer ren;

	[Header("Board")]
	[SerializeField] private int topScore;
	[SerializeField] private int scoreGap; // So there'll be one 'opponent' to beat for every tile you match
	[SerializeField] private int maxNameLength;


	private int totalCount;
	private int lastScore;
	private Save.NFTMatchSaveClass save;

	private void Awake() {
		totalCount = topScore / scoreGap;

		save = Simulation.currentSave.NFTMatchSave;
		if (Simulation.firstGamePlay) {
			FirstPlay();
		}

		ren.Render(save.scoreboard, save.score);
	}

	private void FixedUpdate() {
		if (save.score != lastScore) {
			ren.Render(save.scoreboard, save.score);

			lastScore = save.score;
		}
	}

	public class ScoreboardItem {
		public string username;
		public int score;

		public ScoreboardItem(string _username, int _score) {
			username = _username;
			score = _score;
		}
	}

	private void FirstPlay() {
		save.scoreboard = new ScoreboardItem[totalCount];

		int score = topScore;
		for (int i = 0; i < totalCount; i++) {
			save.scoreboard[i] = new ScoreboardItem(Tools.Random.Username(maxNameLength), score);
			score -= scoreGap;
		}
	}
}
