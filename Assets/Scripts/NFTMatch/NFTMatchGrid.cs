using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchGrid : MonoBehaviour {
	[Header("Objects and references")]
    [SerializeField] private GameObject NFTPrefab;
	[SerializeField] private TextAsset gridDataAsset;

	[Header("Misc")]
	[SerializeField] private int size;

	private GameObject[] NFTGrid;

	private enum Types {
		Red,
		Green,
		Blue,
		Yellow
	}
	private Types[] NFTGridData;
	private Types[] startData;

	private void Awake() {
		string data = gridDataAsset.text;
		startData = new Types[size * size];
		for (int i = 0; i < data.Length; i++) {
			startData[i] = (Types)int.Parse(data[i].ToString());
		}

		GenerateGrid();
		RenderGrid();
	}

	private void GenerateGrid() {
		NFTGridData = (Types[])startData.Clone();
		// TODO: rotate and flip randomly
	}
	private void RenderGrid() {
		NFTGrid = new GameObject[size * size];

		int end = size / 2;
		int start = -end;

		int i = 0;
		for (int y = end; y > start; y--) {
			for (int x = start; x < end; x++) {
				GameObject NFT = Instantiate(NFTPrefab);
				NFT.transform.parent = transform;
				NFT.transform.position = new Vector2(x + 0.5f, y - 0.5f);
				NFTMatchNFT NFTScript = NFT.GetComponent<NFTMatchNFT>();
				NFTScript.type = (NFTMatchNFT.Types)NFTGridData[i];
				NFTScript.Ready();

				NFTGrid[i] = NFT;

				i++;
			}
		}
	}
}
