using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchNFT : MonoBehaviour {
	[SerializeField] private float speed;

	[HideInInspector] public NFTMatchGrid.SquareType type;
	[HideInInspector] public int id;
	[HideInInspector] public NFTMatchGrid dataScript;

	private GameObject visible;
	private SpriteRenderer ren;
	private Animator anim;

	private Vector2 target;
	private bool targetSet;

	private Vector2 offset = new Vector2(0.5f, -0.5f);

	public void ChangeTarget(Vector2Int _target) {
		if (Vector2.Distance(target, _target) > 0.05f || (! targetSet)) {
			target = _target + offset;
			if (targetSet) {
				animating = true;
			}
			else {
				transform.position = target;
				targetSet = true;

				ren.enabled = true; // Make it visible now that the target is set and it's positioned properly
			}
		}
	}
	public void ChangeTargetDir(Vector2Int dir) {
		ChangeTarget(new Vector2Int((int)(target.x - offset.x), (int)(target.y - offset.y)) + dir);
	}

	public void SetFallOffset(int amount) {
		Vector3 pos = transform.position;
		pos.y = ((dataScript.size / 2) + amount) + (offset.y + 1);
		transform.position = pos;

		animating = true;
	}

	[HideInInspector] public bool deleted;
	[HideInInspector] public bool animating { get; private set; }

	private void Awake() {
		visible = transform.GetChild(0).gameObject;

		ren = visible.GetComponent<SpriteRenderer>();
		anim = visible.GetComponent<Animator>();

		Position();
	}

	public void Ready() {
		Color[] colors = {
			Color.red,
			Color.green,
			Color.blue,
			Color.yellow,
			new Color(1f, 119 / 255f, 1f),
			new Color(55 / 255f, 197 / 255f, 1f)
		};

		ren.color = colors[(int)type];
	}

	private void Update() {
		Position();
	}

	private void Position() {
		if (animating) {
			if (Vector2.Distance(transform.position, target) < 0.05f) { // At target
				animating = false;
				transform.position = target;
			}
			else {
				Vector2 newPos = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
				transform.position = newPos;
			}
		}

		if (deleted) {
			anim.SetBool("Delete", true);
			ren.sortingLayerName = "Effects";
		}
	}
}
