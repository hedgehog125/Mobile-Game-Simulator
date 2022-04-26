using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save {
	public int gamesUnlocked = 1;

	public class WatchedClass {
		public bool intro;
	}
	public WatchedClass watched = new WatchedClass();
	
	public int spent;
	public int spendTarget = 1000000;
	public class NFTMatchSaveClass {
		public int matchesUntilNFT; // Initialised when opened
		public int plays = -1; // The 1st is the cutscene which doesn't count as a play
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
}
