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
	public class MiniGameSave {
		public List<OwnedNFT> NFTs = new List<OwnedNFT>();
	}

	public static class miniGameSaves {
		public static MiniGameSave NFTSave = new MiniGameSave();
	}
}
