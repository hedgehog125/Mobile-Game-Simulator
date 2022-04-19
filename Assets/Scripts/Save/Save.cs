using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save {
	public int gamesUnlocked = 1;

	public class WatchedClass {
		public bool intro;
	}
	public WatchedClass watched = new WatchedClass();

    public class OwnedNFT {
		public enum Collections {
			DNG
		}
		public Collections collection;
		public int baseID;
		public int variationID;

		public OwnedNFT(Collections inputCollection, int inputBaseID, int inputVariationID) {
			collection = inputCollection;
			baseID = inputBaseID;
			variationID = inputVariationID;
		}
	}
	public List<OwnedNFT> ownedNFTs = new List<OwnedNFT>();
	
	public int spent;
	public int spendTarget = 1000000;
	public class NFTMatchSaveClass {
		public int matchesUntilNFT; // Initialised when opened
		public int plays = 0; // Note that this is one more than the actual due to the intro cutscene
	}
	public NFTMatchSaveClass NFTMatchSave = new NFTMatchSaveClass();

	public class DNGSaveClass {
		public int dailyLimitProgress;
	}
	public DNGSaveClass DNGSave = new DNGSaveClass();
}
