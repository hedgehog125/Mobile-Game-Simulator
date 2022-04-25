using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTNPGenerator : MonoBehaviour {
    [Header("Objects and References")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private GameObject grassTile;
    [SerializeField] private GameObject fencePrefab;

    [Header("Cutoffs")]
    [SerializeField] private float cutoffTop;
    [SerializeField] private float cutoffBottom;
    [SerializeField] private float buffer;

    [Header("Misc")]
    [SerializeField] private int chunkSize;
    [SerializeField] private int widthInTiles;

    private GameObject centerCube;
    private BoxCollider col;

    private float minGeneratedZ; // Downwards on the 2d screen. Also the lowest anchor
    private float maxGeneratedZ; // Upwards on the 2d screen. Also the highest anchor
    

	private void Awake() {
        centerCube = GameObject.Find("CameraTargetCenter");
        col = GetComponent<BoxCollider>();
        col.size = new Vector3(chunkSize * widthInTiles, col.size.y, chunkSize);

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
                PlaceTile(tilesOffset + new Vector3(x, 0, y), grassTile, chunk.transform);
            }
        }
        ExpandGroundCollider(position);

        if (i == 0 || i == widthInTiles - 1) {
            PlaceFence(position, i != 0, chunk.transform);
        }
    }

    private void PlaceTile(Vector3 position, GameObject prefab, Transform chunkTransform) {
        GameObject ground = Instantiate(prefab, position, Quaternion.identity);
        ground.transform.parent = chunkTransform;
    }

    private void ExpandGroundCollider(Vector3 position) {
        // Unity bad so I have to combine the colliders myself. Even Unity 2D had this feature
        if (! col.bounds.Contains(position)) {
            col.size = new Vector3(col.size.x, col.size.y, col.size.z + chunkSize);
            col.center = new Vector3(col.center.x, col.center.y, col.center.z + (chunkSize / 2));
        }
    }

    private void PlaceFence(Vector3 position, bool side, Transform chunkTransform) {
        GameObject fence = Instantiate(fencePrefab, position + new Vector3((chunkSize / 2) * (side? 1 : -1), 0.25f, 0), Quaternion.identity);
        fence.transform.parent = chunkTransform;
    }
}
