using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save {
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
		public bool opened = false;
	}
	public NFTMatchSaveClass NFTMatchSave = new NFTMatchSaveClass();

	public class DNGSaveClass {
		public int dailyLimitProgress;
	}
	public DNGSaveClass DNGSave = new DNGSaveClass();
}
