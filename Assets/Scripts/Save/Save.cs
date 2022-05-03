using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save {

	public class WatchedClass {
		public bool intro;
	}
	public WatchedClass watched = new WatchedClass();
	
	public int spent;
	public int knowledgePoints;
	public int timeLeft;
	public int gamesUnlocked = 1;
	public int difficultyLevel = 1;

	public bool[] knowledgePointsGot = new bool[1];

	public class NFTMatchSaveClass {
		public int plays;
	}
	public NFTMatchSaveClass NFTMatchSave = new NFTMatchSaveClass();

	public class DNGSaveClass {
		public int dailyLimitProgress;

		public class OwnedNFT {
			public int baseID;
			public int variationID;

			public OwnedNFT(int inputBaseID, int inputVariationID) {
				baseID = inputBaseID;
				variationID = inputVariationID;
			}
		}
		public List<OwnedNFT> ownedNFTs = new List<OwnedNFT>();
	}
	public DNGSaveClass DNGSave = new DNGSaveClass();

	public Save() {
		Simulation.Difficulty difficulty = new Simulation.Difficulty(difficultyLevel);

		timeLeft = difficulty.gameTimeLimit;
	}
}
