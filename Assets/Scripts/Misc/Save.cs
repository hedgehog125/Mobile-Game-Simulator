using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour {
    public class OwnedNFT {
		public enum Collections {
			DNG
		}
		public Collections collection;
		public int baseID;
		public int variationID;
	}
	public class MiniGame {
		public List<OwnedNFT> NFTs = new List<OwnedNFT>();
	}

	public static MiniGame[] games = new MiniGame[1];
}
