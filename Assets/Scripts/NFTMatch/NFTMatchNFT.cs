using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NFTMatchNFT : MonoBehaviour {
	[Header("Objects and references")]
	[SerializeField] private List<Sprite> NFTs;
	[SerializeField] private AudioSource matchSound;

	[HideInInspector] public NFTMatchGrid.SquareType type;
	[HideInInspector] public int id;
	[HideInInspector] public NFTMatchGrid dataScript;
	[HideInInspector] public NFTMatchRenderer parentScript;
	[HideInInspector] public bool shake;
	[HideInInspector] public bool shakeDir;
	[HideInInspector] public bool swapping;

	private GameObject visible;
	private SpriteRenderer ren;
	private Animator anim;

	private Vector2 target;
	private bool targetSet;

	private bool falling;
	private bool deleteStarted;

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

			if (target.y < transform.position.y && (! swapping)) {
				falling = true;
			}
		}
	}
	public void ChangeTargetDir(Vector2Int dir) {
		ChangeTarget(new Vector2Int((int)(target.x - offset.x), (int)(target.y - offset.y)) + dir);
	}

	public void SetFallOffset(int amount) {
		Vector3 pos = transform.position;
		pos.y = ((dataScript.size / 2) + amount) + (offset.y + 1);
		pos.y += 0.5f;
		transform.position = pos;

		animating = true;
		falling = true;
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

		//ren.color = colors[(int)type];
		ren.sprite = NFTs[(int)type];
	}

	private void Update() {
		Position();
	}

	private void Position() {
		if (animating) {
			if (Vector2.Distance(transform.position, target) < 0.05f) { // At target
				Finish();
			}
			else {
				float speed = falling? parentScript.fallSpeed : parentScript.speed;

				Vector2 newPos = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
				transform.position = newPos;
			}
		}

		if (deleted) {
			if (! deleteStarted) {
				anim.SetBool("Delete", true);
				ren.sortingLayerName = "Effects";
				matchSound.Play();

				deleteStarted = true;
			}
		}
	}

	private void Finish() {
		animating = false;
		falling = false;
		swapping = false;

		if (shake) {
			anim.SetTrigger("Shake");
			anim.SetBool("ShakeDir", shakeDir);
		}

		transform.position = target;
	}
}
