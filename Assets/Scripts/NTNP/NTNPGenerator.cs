using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTNPGenerator : MonoBehaviour {
    [Header("Objects and References")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private List<GameObject> tiles;

    [Header("Cutoffs")]
    [SerializeField] private float cutoffTop;
    [SerializeField] private float cutoffBottom;
    [SerializeField] private float buffer;

    [Header("Misc")]
    [SerializeField] private int chunkSize;
    [SerializeField] private int widthInTiles;

    private GameObject centerCube;

    private float minGeneratedZ; // Downwards on the 2d screen
    private float maxGeneratedZ; // Upwards on the 2d screen
    

	private void Awake() {
        centerCube = GameObject.Find("CameraTargetCenter");
        maxGeneratedZ += chunkSize; // So they don't both try and generate the first row

        Tick();
	}

	private void FixedUpdate() {
        Tick();
	}

	private void Tick() {
        float bufferAndLength = buffer + (chunkSize / 2);
        float center = centerCube.transform.position.z;

        float minVisibleZ = (center + cutoffBottom) - bufferAndLength;
        float maxVisibleZ = (center + cutoffTop) + bufferAndLength;

        while (minVisibleZ < minGeneratedZ) {
            GenerateRow(minGeneratedZ);
            minGeneratedZ -= chunkSize;
        }
        while (maxVisibleZ > maxGeneratedZ) {
            GenerateRow(maxGeneratedZ);
            maxGeneratedZ += chunkSize;
        }
    }

    private void GenerateRow(float offset) {
        float x = -((chunkSize * (widthInTiles - 1)) / 2);
        for (int i = 0; i < widthInTiles; i++) {
            Vector3 position = new Vector3(x, 0, offset);
            GenerateChunk(position, i);

            x += chunkSize;
		}
	}

    private void GenerateChunk(Vector3 position, int i) {
        GameObject chunk = Instantiate(chunkPrefab, position, Quaternion.identity);
        chunk.transform.parent = transform;

        Vector3 tilesOffset = position - new Vector3(chunkSize / 2, 0, chunkSize / 2);
        for (int y = 0; y < chunkSize; y++) {
            for (int x = 0; x < chunkSize; x++) {
                PlaceTile(0, tilesOffset + new Vector3(x, 0, y), chunk.transform);
            }
        }

        if (i == 0 || i == widthInTiles - 1) {
            PlaceFence(position, i != 0, chunk.transform);
        }
    }

    private void PlaceTile(int tileID, Vector3 position, Transform chunkTransform) {
        GameObject prefab = tiles[tileID];

        GameObject ground = Instantiate(prefab, position, Quaternion.identity);
        ground.transform.parent = chunkTransform;
        ground.GetComponent<NTNPTile>().Ready();
    }

    private void PlaceFence(Vector3 position, bool side, Transform chunkTransform) {
        PlaceTile(1, position + new Vector3((chunkSize / 2) * (side ? 1 : -1), 0.25f, 0), chunkTransform);
    }
}
